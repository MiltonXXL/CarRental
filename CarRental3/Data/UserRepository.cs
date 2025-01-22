using CarRental3.Models;

namespace CarRental3.Data
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDbContext userRepository;

        public UserRepository(ApplicationDbContext userRepository)
        {
            this.userRepository = userRepository;
        }
        public void Add(User user)
        {
            userRepository.Add(user);
            userRepository.SaveChanges();
        }

        public void Delete(User user)
        {
            userRepository.Remove(user);
            userRepository.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return (userRepository.Users.OrderBy(u => u.UserName));
        }

        public User GetById(int id)
        {
            return (userRepository.Users.Find(id));
        }

        public void Update(User user)
        {
            userRepository.Update(user);
            userRepository.SaveChanges();
        }
    }
}
