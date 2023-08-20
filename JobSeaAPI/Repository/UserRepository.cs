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
        private IMapper _mapper;
        private IPasswordHelper _passwordHelper;
        public UserRepository(ApplicationDbContext db, ILoggerCustom logger, IMapper mapper, IPasswordHelper passwordHelper) : base(db, logger)
        {
            _mapper = mapper;
            _passwordHelper = passwordHelper;
        }

        public UserDTO? Authenticate(string username, string password)
        {
            User? foundUser = GetEntity(user => user.Username == username) ?? throw new JobSeaException(System.Net.HttpStatusCode.NotFound, "User not found.");
            bool match = _passwordHelper.VerifyPassword(password, foundUser.password, foundUser.passwordSalt);

            if (match is false)
            {
                throw new JobSeaException(System.Net.HttpStatusCode.Unauthorized, "Invalid credentials");
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
            User user = GetEntity(user => user.UserId == userId) ?? throw new JobSeaException(System.Net.HttpStatusCode.NotFound, "User not found.");
            return user;
        }
        public async Task CreateUser(User user)
        {
            user.password = _passwordHelper.HashPassword(user.password, out byte[] salt);
            user.passwordSalt = salt;
            await CreateEntity(user);
        }
        public async Task<User> UpdateUser(UpdateUserDTO userDTO)
        {
            Expression<Func<User, bool>> expression = entity =>  entity.UserId == userDTO.UserId;
            User user = GetEntity(expression) ?? throw new JobSeaException(System.Net.HttpStatusCode.NotFound, "UserId does not match any entity in database.");

            await UpdateEntity(user, userDTO);
            return user;
        }

        public async Task DeleteUser(int userId)
        {
            User user = GetEntity(user => user.UserId == userId) ?? throw new JobSeaException(System.Net.HttpStatusCode.NotFound, "User not found.");
            if (user is not null) await DeleteEntity(user);
        }
    }
}
