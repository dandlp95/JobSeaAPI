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

        public async Task<UserDTO> Authenticate(string username, string password)
        {
            User foundUser = await GetAsync(user => user.Username == username);
            // Password will be hashed.
            if (foundUser == null || foundUser.password != password)
            {
                return null;
            }
            UserDTO loggedinUser = _mapper.Map<UserDTO>(foundUser);
            return loggedinUser;
        }
    }
}
