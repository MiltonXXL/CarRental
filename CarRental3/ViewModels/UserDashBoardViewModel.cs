using CarRental3.Models;
using System.Collections.Generic;

namespace CarRental3.ViewModels
{
    public class UserDashBoardViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
        public Booking NewBooking { get; set; }
        public IEnumerable<Car> Cars { get; set; }
    }
}
