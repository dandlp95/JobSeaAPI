using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        User? Authenticate(string username, string password);
        List<User> GetAllUsers();
        User GetUser(int userId);
        void CreateUser(User user);
    }
}
