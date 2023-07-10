using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        UserDTO? Authenticate(string username, string password);
        List<User> GetAllUsers();
        User GetUser(int userId);
        Task CreateUser(User user);
    }
}
