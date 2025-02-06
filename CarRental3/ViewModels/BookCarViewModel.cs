namespace CarRental3.ViewModels
{
    public class BookCarViewModel
    {
        public int? CarId { get; set; }
        public AvailableCarsLoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
    }
}
