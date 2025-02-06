using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string RegisterUserName { get; set; }
        [Required]
        public string RegisterPassword { get; set; }

        public int? CarId { get; set; } // Lägg till CarId för att bevara bilens ID
    }
}
