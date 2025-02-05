namespace CarRental3.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int YearModel { get; set; }
        public double CostPerDay { get; set; }

        public string ImageUrl { get; set; }
    }
}
