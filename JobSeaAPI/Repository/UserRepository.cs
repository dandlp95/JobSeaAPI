using AutoMapper;
using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;

namespace JobSeaAPI.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext db, ILoggerCustom logger, IMapper mapper) : base(db, logger)
        {
            _db = db;
            dbSet = db.Set<User>();
            _mapper = mapper;
        }

        public User? Authenticate(string username, string password)
        {
            User foundUser = GetEntity(user => user.Username == username);
            // Password will be hashed.
            if (foundUser == null || foundUser.password != password)
            {
                return null;
            }
            return foundUser;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = GetAllEntities();
            return users;
        }

        public User GetUser(int userId)
        {
            User user = GetEntity(user => user.UserId == userId);
            return user;
        }
        public async void CreateUser(User user)
        {
            await CreateEntity(user);
        }
        
    }
}
