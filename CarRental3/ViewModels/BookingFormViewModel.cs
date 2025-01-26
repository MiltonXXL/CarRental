using CarRental3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class BookingFormViewModel
    {
        [Required]
        public int CarId { get; set; }
        public IEnumerable<Car> Cars { get; set; }

        [Required]
        public int UserId { get; set; }
        public IEnumerable<User> Users { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
