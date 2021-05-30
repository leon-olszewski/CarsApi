using CarsApi.Data;
using CarsApi.Dtos;
using CarsApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;

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
            try
            {
                ValidateCarForCreate(carForCreate);
            }
            catch (Exception e)
            {
                return BadRequest("Invalid input. " + e.Message);
            }
          
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

        private void ValidateCarForCreate(CarForCreateDto carForCreate)
        {
            if (carForCreate.Make == null)
                throw new Exception("Make must be provided.");

            if (carForCreate.Model == null)
                throw new Exception("Model must be provided.");

            if (carForCreate.Year == null)
                throw new Exception("Year must be provided.");

            if (carForCreate.Year <= 0)
                throw new Exception("Year must be positive.");

            if (carForCreate.Color == null)
                throw new Exception("Color must be provided.");

            if (carForCreate.VehicleIdentificationNumber == null)
                throw new Exception("Vehicle identification number must be provided.");
        }

        private Car MapToModel(CarForCreateDto carDto)
        {
            return new Car(
                carDto.Make!,
                carDto.Model!,
                carDto.Year!.Value,
                carDto.Color!,
                carDto.VehicleIdentificationNumber!
            );
        }
    }
}
