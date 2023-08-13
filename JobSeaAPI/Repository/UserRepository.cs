using AutoMapper;
using JobSeaAPI.Database;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using System.Linq.Expressions;

namespace JobSeaAPI.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        IMapper _mapper;
        ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db, ILoggerCustom logger, IMapper mapper) : base(db, logger)
        {
            _mapper = mapper;
            _db = db;
        }

        public UserDTO? Authenticate(string username, string password)
        {
            User? foundUser = GetEntity(user => user.Username == username);
            // Password will be hashed.
            if (foundUser == null || foundUser.password != password)
            {
                return null;
            }
            UserDTO responseUser = _mapper.Map<UserDTO>(foundUser);
            return responseUser;
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
        public async Task CreateUser(User user)
        {
            await CreateEntity(user);
        }
        public async Task<User> UpdateUser(UpdateUserDTO userDTO)
        {
            Expression<Func<User, bool>> expression = entity =>  entity.UserId == userDTO.UserId;
            User? user = GetEntity(expression) ?? throw new JobSeaException(System.Net.HttpStatusCode.BadRequest, "UserId does not match any entity in database.");

            if (!string.IsNullOrEmpty(userDTO.Username)) user.Username = userDTO.Username;
            if (!string.IsNullOrEmpty(userDTO.email)) user.email = userDTO.email;
            if (!string.IsNullOrEmpty(userDTO.Password)) user.password = userDTO.Password;

            await _db.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUser(int userId)
        {
            User? user = GetEntity(user => user.UserId == userId);
            if (user is not null) await DeleteEntity(user);
        }
    }
}
