using CarRental3.Models;

namespace CarRental3.Data
{
    public class AdministratorRepository : IAdministrator
    {
        private readonly ApplicationDbContext dbContext;

        public AdministratorRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Administrator administrator)
        {
            dbContext.Admins.Add(administrator);
            dbContext.SaveChanges();
        }

        public void Delete(Administrator administrator)
        {
            dbContext.Admins.Remove(administrator);
            dbContext.SaveChanges();
        }

        public IEnumerable<Administrator> GetAll()
        {
            return dbContext.Admins.OrderBy(a => a.Name);
        }

        public Administrator GetById(int id)
        {
            return dbContext.Admins.Find(id);
        }

        public void Update(Administrator administrator)
        {
            dbContext.Update(administrator);
            dbContext.SaveChanges();
        }
    }
}
