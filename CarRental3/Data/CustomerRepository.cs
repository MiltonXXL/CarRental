using CarRental3.Models;

namespace CarRental3.Data
{
    public class CustomerRepository : ICustomer
    {
        private readonly ApplicationDbContext customerRepository;

        public CustomerRepository(ApplicationDbContext customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public void Add(Customer customer)
        {
            customerRepository.Add(customer);
            customerRepository.SaveChanges();
        }

        public void Delete(Customer customer)
        {
            customerRepository.Remove(customer);
            customerRepository.SaveChanges();
        }

        public IEnumerable<Customer> GetAll()
        {
            return (customerRepository.Customers);
        }

        public Customer GetById(int id)
        {
            return (customerRepository.Customers.Find(id));
        }

        public void Update(Customer customer)
        {
            customerRepository.Update(customer);
            customerRepository.SaveChanges();
        }
    }
}
