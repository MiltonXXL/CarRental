using CarRental3.Models;

namespace CarRental3.Data
{
    public interface IUser
    {
        User GetById(int id);
        User GetByUserName(string userName);
        User GetByUserNameAndPassword(string userName, string password);
        IEnumerable<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
