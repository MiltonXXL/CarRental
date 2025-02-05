using CarRental3.Models;

namespace CarRental3.Data
{
    public interface ICar
    {
        Car GetById(int id);
        IEnumerable<Car> GetAll();
        bool HasActiveOrFutureBooking(int carId);
        void Add(Car car);
        void Update(Car car);
        void Delete(Car car);
    }
}
