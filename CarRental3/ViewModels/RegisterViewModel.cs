using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public int CarId { get; set; } // Lägg till CarId för att bevara bilens ID
    }
}
