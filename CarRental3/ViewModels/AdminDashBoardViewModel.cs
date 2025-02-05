using CarRental3.Models;

namespace CarRental3.ViewModels
{
    public class AdminDashBoardViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
