using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using System.Linq.Expressions;

namespace JobSeaAPI.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext db, ILoggerCustom logger) : base(db, logger)
        {
        }
    }
}
