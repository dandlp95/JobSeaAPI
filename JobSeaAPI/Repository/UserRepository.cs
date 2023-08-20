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
        public UserRepository(ApplicationDbContext db, ILoggerCustom logger, IMapper mapper) : base(db, logger)
        {
            _mapper = mapper;
        }

        public UserDTO? Authenticate(string username, string password)
        {
            User? foundUser = GetEntity(user => user.Username == username);

            if (foundUser == null)
            {
                throw new JobSeaException(System.Net.HttpStatusCode.NotFound, "User not found.");
            }
            else if (foundUser.password != password)
            {
                throw new JobSeaException(System.Net.HttpStatusCode.Unauthorized, "Invalid credentials.");
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
            await CreateEntity(user);
        }
        public async Task<User> UpdateUser(UpdateUserDTO userDTO)
        {
            Expression<Func<User, bool>> expression = entity =>  entity.UserId == userDTO.UserId;
            User user = GetEntity(expression) ?? throw new JobSeaException(System.Net.HttpStatusCode.BadRequest, "UserId does not match any entity in database.");

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
