using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarRental3.Data
{
    public class BookingRepository : IBooking
    {
        private readonly ApplicationDbContext dbContext;

        public BookingRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(Booking booking)
        {
            dbContext.Bookings.Add(booking);
            dbContext.SaveChanges();
        }

        public void Delete(Booking booking)
        {
            dbContext.Bookings.Remove(booking);
            dbContext.SaveChanges();
        }

        public IEnumerable<Booking> GetAll()
        {
            return dbContext.Bookings.OrderBy(b => b.BookingId);
        }

        public IEnumerable<Car> GetAllCars()
        {
            return dbContext.Cars.ToList();
        }

        public IEnumerable<Booking> GetByCarId(int carId)
        {
            return dbContext.Bookings
                       .Include(b => b.Car)
                       .Include(b => b.User)
                       .Where(b => b.CarId == carId)
                       .ToList();
        }

        public Booking GetById(int id)
        {
            return dbContext.Bookings
                       .Include(b => b.Car) // Ladda relaterad Car
                       .Include(b => b.User) // Ladda relaterad User
                       .FirstOrDefault(b => b.BookingId == id);
        }
        public IEnumerable<Booking> GetByUserId(int userId)
        {
            return dbContext.Bookings
                .Include(b => b.Car)
                .Include(b =>b.User)
                .Where(b => b.UserId == userId)
                .ToList();
        }

        public void Update(Booking booking)
        {
            dbContext.Bookings.Update(booking);
            dbContext.SaveChanges();
        }
    }
}
