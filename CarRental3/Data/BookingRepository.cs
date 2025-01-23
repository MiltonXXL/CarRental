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
            bookingRepository.Bookings.Add(booking);
            bookingRepository.SaveChanges();
        }

        public void Delete(Booking booking)
        {
            bookingRepository.Bookings.Remove(booking);
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
            bookingRepository.Bookings.Update(booking);
            bookingRepository.SaveChanges();
        }
    }
}
