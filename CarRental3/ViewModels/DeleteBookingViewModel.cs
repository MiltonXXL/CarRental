using CarRental3.Models;

namespace CarRental3.ViewModels
{
    public class DeleteBookingViewModel
    {
        public int UserId { get; set; }
        public Booking Booking { get; set; }
    }
}
