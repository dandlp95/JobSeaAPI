using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobSeaAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public TokenService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public string GetToken(string username, int userId)
        {
            var secretKey = _configuration["AppSettings:SecretKey"];
            var apiURL = _configuration["AppSettings:ApiUrl"];
            var Claims = new List<Claim>
            {
                new Claim("type", "User"),
                new Claim("username", username),
                new Claim("userId", userId.ToString())
            };
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var Token = new JwtSecurityToken(
                apiURL,
                apiURL,
                Claims,
                expires: DateTime.Now.AddDays(15.0),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
        public int ValidateUserIdToken(Claim userIdClaim, int userId)
        {
            if(Int32.TryParse(userIdClaim?.Value, out int userIdClaimValue))
            {
                if (userIdClaimValue != userId) return -1;
                User user = _userRepository.GetUser(userId);
                return user is not null ? 1 : 0;
            }
            return 0;
        }
    }
}
