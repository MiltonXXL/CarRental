using CarRental3.Models;

namespace CarRental3.Data
{
    public class AdministratorRepository : IAdministrator
    {
        private readonly ApplicationDbContext administratorDbContext;

        public AdministratorRepository(ApplicationDbContext administratorDbContext)
        {
            this.administratorDbContext = administratorDbContext;
        }

        public void Add(Administrator administrator)
        {
            administratorDbContext.Admins.Add(administrator);
            administratorDbContext.SaveChanges();
        }

        public void Delete(Administrator administrator)
        {
            administratorDbContext.Admins.Remove(administrator);
            administratorDbContext.SaveChanges();
        }

        public IEnumerable<Administrator> GetAll()
        {
            return administratorDbContext.Admins.OrderBy(a => a.Name);
        }

        public Administrator GetById(int id)
        {
            return administratorDbContext.Admins.Find(id);
        }

        public void Update(Administrator administrator)
        {
            administratorDbContext.Update(administrator);
            administratorDbContext.SaveChanges();
        }
    }
}
