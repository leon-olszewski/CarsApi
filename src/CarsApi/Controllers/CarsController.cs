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
            try
            {
                ValidateCarForCreate(carForCreate);
            }
            catch (Exception e)
            {
                return BadRequest("Invalid input. " + e.Message);
            }

            var carModelToCreate = MapToModel(carForCreate);
            var existingCarModel = _carsRepo.GetCarByVin(carModelToCreate.Vin);

            IActionResult response;
            if (existingCarModel != null)
            {
                response = BadRequest("Car with the given VIN already exists.");
            }
            else
            {
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
