using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobSeaAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken()
        {
            var secretKey = _configuration["AppSettings:SecretKey"];
            var apiURL = _configuration["AppSettings:ApiUrl"];
            var Claims = new List<Claim>
            {
                new Claim("type", "User")
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
    }
}
