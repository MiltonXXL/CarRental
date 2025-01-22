using CarRental3.Models;

namespace CarRental3.Data
{
    public interface IBooking
    {
        Booking GetById(int id);
        IEnumerable<Booking> GetAll();
        void Add(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);
    }
}
