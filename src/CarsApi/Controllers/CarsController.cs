using CarsApi.Data;
using CarsApi.Dtos;
using CarsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarsRepository _carsRepo;

        public CarsController(ICarsRepository carsRepo)
        {
            _carsRepo = carsRepo;
        }

        [HttpGet]
        public IActionResult GetAllCars()
        {
            var cars = _carsRepo.GetAllCars();
            return Ok(cars);
        }

        [HttpPost]
        public IActionResult CreateCar([FromBody] CarForCreateDto carForCreate)
        {
            // Validate the input
            var validationResult = ValidateCarForCreate(carForCreate);
            if (!validationResult.IsValid)
                return BadRequest("Invalid input. " + validationResult.ErrorMessage);

            // Map the input to a model
            var carModelToCreate = MapToModel(carForCreate);

            // Check to see if there's already a car with the same
            // VIN in the data store. We don't want duplicates.
            var existingCarModel = _carsRepo.GetCarByVin(carModelToCreate.Vin);

            IActionResult response;
            if (existingCarModel != null)
            {
                // The input is a duplicate car. Fail.
                response = BadRequest("Car with the given VIN already exists.");
            }
            else
            {
                // The input is unique. Proceed with car creation.
                _carsRepo.AddCar(carModelToCreate);
                response = NoContent();
            }

            return response;
        }

        private ValidationResult ValidateCarForCreate(CarForCreateDto carForCreate)
        {
            if (carForCreate.Make == null)
                return ValidationResult.Invalid("Make must be provided.");

            if (carForCreate.Model == null)
                return ValidationResult.Invalid("Model must be provided.");

            if (carForCreate.Year == null)
                return ValidationResult.Invalid("Year must be provided.");

            if (carForCreate.Year <= 0)
                return ValidationResult.Invalid("Year must be positive.");

            if (carForCreate.Color == null)
                return ValidationResult.Invalid("Color must be provided.");

            if (carForCreate.VehicleIdentificationNumber == null)
                return ValidationResult.Invalid("Vehicle identification number must be provided.");

            return ValidationResult.Valid();
        }

        private Car MapToModel(CarForCreateDto carDto)
        {
            return new Car(
                make: carDto.Make!,
                model: carDto.Model!,
                year: carDto.Year!.Value,
                color: carDto.Color!,
                vin: carDto.VehicleIdentificationNumber!
            );
        }

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }

            private ValidationResult(bool isValid, string errorMessage)
            {
                IsValid = isValid;
                ErrorMessage = errorMessage;
            }

            public static ValidationResult Valid() => new ValidationResult(true, string.Empty);
            public static ValidationResult Invalid(string errorMessage) => new ValidationResult(false, errorMessage);
        }
    }
}
