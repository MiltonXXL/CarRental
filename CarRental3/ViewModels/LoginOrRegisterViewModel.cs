using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class LoginOrRegisterViewModel : LoginViewModel
    {
        public int? CarId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RegisterUserName { get; set; }
        public string RegisterPassword { get; set; }


    }
}
