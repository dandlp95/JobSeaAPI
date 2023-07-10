﻿using System.Security.Claims;

namespace JobSeaAPI.Services
{
    public interface ITokenService
    {
        public string GetToken(string username, int userId, bool isAdmin = false);
        public int ValidateUserIdToken(Claim userIdClaim, int userId);
    }
}
