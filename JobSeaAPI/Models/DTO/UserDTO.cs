﻿using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
