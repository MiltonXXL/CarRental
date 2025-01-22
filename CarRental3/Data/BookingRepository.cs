using CarRental3.Models;

namespace CarRental3.Data
{
    public class BookingRepository : IBooking
    {
        private readonly ApplicationDbContext bookingRepository;

        public BookingRepository(ApplicationDbContext bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }
        public void Add(Booking booking)
        {
            bookingRepository.Add(booking);
            bookingRepository.SaveChanges();
        }

        public void Delete(Booking booking)
        {
            bookingRepository.Remove(booking);
            bookingRepository.SaveChanges();
        }

        public IEnumerable<Booking> GetAll()
        {
            return (bookingRepository.Bookings.OrderBy(b => b.BookingId));
        }

        public Booking GetById(int id)
        {
            return (bookingRepository.Bookings.Find(id));
        }

        public void Update(Booking booking)
        {
            bookingRepository.Update(booking);
            bookingRepository.SaveChanges();
        }
    }
}
