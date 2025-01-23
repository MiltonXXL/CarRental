using CarRental3.Models;

namespace CarRental3.Data
{
    public class CarRepository : ICar
    {
        private readonly ApplicationDbContext carRepository;

        public CarRepository(ApplicationDbContext carRepository)
        {
            this.carRepository = carRepository;
        }
        public void Add(Car car)
        {
            carRepository.Cars.Add(car);
            carRepository.SaveChanges();
        }

        public void Delete(Car car)
        {
            carRepository.Cars.Remove(car);
            carRepository.SaveChanges();
        }

        public IEnumerable<Car> GetAll()
        {
            return(carRepository.Cars.OrderBy(c => c.Brand));
        }

        public Car GetById(int id)
        {
            return (carRepository.Cars.Find(id));
        }

        public void Update(Car car)
        {
            carRepository.Cars.Update(car);
            carRepository.SaveChanges();
        }
    }
}
