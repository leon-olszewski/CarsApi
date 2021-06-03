using CarsApi.Models;
using System.Collections.Generic;

namespace CarsApi.Data
{
    public interface ICarsRepository
    {
        void AddCar(Car car);
        IEnumerable<Car> GetAllCars();
        CarFetchResult GetCar(int id);
        CarFetchResult GetCarByVin(string vin);
    }
}
