using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobSeaAPI.Services
{
    public interface ITokenService
    {
        string GetToken(string username, int userId, bool isAdmin = false);
        int ValidateUserIdToken(Claim userIdClaim, int userId);
        ActionResult tokenValidationResponseAction(Claim userIdClaim, int userId, APIResponse response);
    }
}
