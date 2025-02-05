using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental3.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigerings-egenskaper
        public Car Car { get; set; }
        public User User { get; set; }
    }
}
