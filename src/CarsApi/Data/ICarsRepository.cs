using CarsApi.Models;
using System.Collections.Generic;

namespace CarsApi.Data
{
    public interface ICarsRepository
    {
        void AddCar(Car car);
        IEnumerable<Car> GetAllCars();
        Car? GetCar(int id);
        Car? GetCarByVin(string vin);
    }
}
