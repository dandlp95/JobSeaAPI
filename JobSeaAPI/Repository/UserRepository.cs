using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using System.Linq.Expressions;

namespace JobSeaAPI.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
