using CarsApi.Data;
using CarsApi.Dtos;
using CarsApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
            if (cars == null)
            {
                return Ok(new List<Car>());
            }
            else
            {
                return Ok(cars);
            }
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

            var m = MapToModel(carForCreate);
            var c = _carsRepo.GetCarByVin(m.Vin);

            IActionResult response;
            if (c != null)
            {
                response = BadRequest("Car with the given VIN already exists.");
            }
            else
            {
                _carsRepo.AddCar(m);
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
                carDto.Make,
                carDto.Model,
                carDto.Year.Value,
                carDto.Color,
                carDto.VehicleIdentificationNumber
            );
        }
    }
}
