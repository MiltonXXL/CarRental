using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public int? CarId { get; set; }
    }
}
