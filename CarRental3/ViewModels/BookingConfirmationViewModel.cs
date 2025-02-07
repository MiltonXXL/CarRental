namespace CarRental3.ViewModels
{
    public class BookingConfirmationViewModel
    {
        public int BookingId { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Email { get; set; } 
    }
}
