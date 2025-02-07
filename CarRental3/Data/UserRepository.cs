using CarRental3.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental3.Data
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public void Delete(User user)
        {
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return (dbContext.Users.OrderBy(u => u.Email));
        }

        public User GetById(int id)
        {
            return dbContext.Users.FirstOrDefault(u => u.UserId == id);
        }

        public User GetByUserNameAndPassword(string userName, string password)
        {
            return dbContext.Users.FirstOrDefault(u => u.Email == userName && u.Password == password);
        }

        public User GetByUserName(string userName)
        {
            return dbContext.Users.FirstOrDefault(u => u.Email == userName);
        }

        public void Update(User user)
        {
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
        }
    }
}
