using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<UserDTO> Authenticate(string username, string password);
    }
}
