using System.Security.Claims;

namespace JobSeaAPI.Services
{
    public interface ITokenService
    {
        public string GetToken(string username, int userId);
        public int ValidateUserIdToken(Claim userIdClaim);
    }
}
