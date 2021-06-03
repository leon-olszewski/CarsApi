using CarsApi.Models;

namespace CarsApi
{
    public class CarFetchResult
    {
        public bool IsFound { get; }
        public Car? Car { get; }

        private CarFetchResult(bool isFound, Car? car)
        {
            IsFound = isFound;
            Car = car;
        }

        public static CarFetchResult Found(Car car) => new CarFetchResult(true, car);
        public static CarFetchResult NotFound() => new CarFetchResult(false, default);
    }
}
