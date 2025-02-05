using CarRental3.Models;

namespace CarRental3.ViewModels
{
    public class BookingDetailsViewModel
    {
        public Booking Booking { get; set; }
        public User User { get; set; }
        public Car Car { get; set; }
    }
}
