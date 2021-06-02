using CarsApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarsApi.Data
{
    public class CarsRepository : ICarsRepository
    {
        private int _nextId = 1;
        private List<Car> _cars = new();

        public void AddCar(Car car)
        {
            car.Id = _nextId++;
            _cars.Add(car);
        }

        public IEnumerable<Car> GetAllCars() => _cars;

        public Car GetCar(int id) => _cars
            .FirstOrDefault(c => c.Id == id);

        public Car GetCarByVin(string vin) => _cars
            .FirstOrDefault(c => c.Vin == vin);
    }
}
