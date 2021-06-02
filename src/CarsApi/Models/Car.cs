namespace CarsApi.Models
{
    public class Car
    {
        public Car(string make, string model, int year, string color, string vin)
        {
            Make = make;
            Model = model;
            Year = year;
            Color = color;
            Vin = vin;
        }

        public int Id { get; set; }
        public string Make { get; }
        public string Model { get; }
        public int Year { get; }
        public string Color { get; }
        public string Vin { get; }
    }
}
