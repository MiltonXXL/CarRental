using CarRental3.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental3.Data
{
    public class CarRepository : ICar
    {
        private readonly ApplicationDbContext dbContext;

        public CarRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(Car car)
        {
            if (string.IsNullOrEmpty(car.ImageUrl))
            {
                car.ImageUrl = "/images/cars/default_car.jpg";  
            }

            dbContext.Cars.Add(car);
            dbContext.SaveChanges();
        }


        public void Delete(Car car)
        {
            dbContext.Cars.Remove(car);
            dbContext.SaveChanges();
        }

        public IEnumerable<Car> GetAll()
        {
            return(dbContext.Cars.OrderBy(c => c.Brand));
        }

        public Car GetById(int id)
        {
            return (dbContext.Cars.Find(id));
        }

        public void Update(Car car)
        {
            dbContext.Cars.Update(car);
            dbContext.SaveChanges();
        }

        public bool HasActiveOrFutureBooking(int carId)
        {
            var currentDate = DateTime.Now;
            return dbContext.Bookings.Any(b => b.CarId == carId && b.EndDate >= currentDate);
        }

    }
}
