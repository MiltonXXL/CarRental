using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class LoginOrRegisterViewModel : LoginViewModel
    {
        public int? CarId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string RegisterEmail { get; set; }
        public string RegisterPassword { get; set; }


    }
}
