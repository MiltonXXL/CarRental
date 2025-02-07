using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
