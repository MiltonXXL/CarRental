using CarRental3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public IEnumerable<SelectListItem> Cars { get; set; } // Dropdown med bilar
        public IEnumerable<SelectListItem> Users { get; set; }

        public Car Car { get; set; }
        public User User { get; set; }

        public string ImageUrl { get; set; }
        public string UserRole { get; set; } // Lägg till en egenskap för användarens roll

    }
}
