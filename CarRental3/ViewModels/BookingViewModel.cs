using CarRental3.Models;

namespace CarRental3.ViewModels
{
    public class BookingViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
