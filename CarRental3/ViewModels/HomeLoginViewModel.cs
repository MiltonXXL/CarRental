using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class HomeLoginViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
