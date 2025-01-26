namespace CarRental3.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //Foreign keys
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
