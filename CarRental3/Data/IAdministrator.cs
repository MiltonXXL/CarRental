using CarRental3.Models;

namespace CarRental3.Data
{
    public interface IAdministrator
    {
        Administrator GetById(int id);
        IEnumerable<Administrator> GetAll();
        void Add(Administrator administrator);
        void Update(Administrator administrator);
        void Delete(Administrator administrator);
    }
}
