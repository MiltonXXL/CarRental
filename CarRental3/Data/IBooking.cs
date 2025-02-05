using CarRental3.Models;
using CarRental3.ViewModels;

namespace CarRental3.Data
{
    public interface IBooking
    {
        Booking GetById(int id);
        IEnumerable<Booking> GetAll();
        IEnumerable<Booking> GetByUserId(int userId);
        IEnumerable<Booking> GetByCarId(int carId);
        IEnumerable<Car> GetAllCars();
        void Add(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);
    }
}
