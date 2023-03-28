using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Services
{
    public interface ITokenService
    {
        public string GetToken(UserDTO userInfo);
    }
}
