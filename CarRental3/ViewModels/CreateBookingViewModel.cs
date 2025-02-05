using CarRental3.Models;
using System.Collections.Generic;

namespace CarRental3.ViewModels
{
    public class CreateBookingViewModel
    {
        public Booking Booking { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Car> Cars { get; set; }
    }
}
