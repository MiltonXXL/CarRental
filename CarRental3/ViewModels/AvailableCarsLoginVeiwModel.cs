using System.ComponentModel.DataAnnotations;

namespace CarRental3.ViewModels
{
    public class AvailableCarsLoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CarId { get; set; }
    }
}
