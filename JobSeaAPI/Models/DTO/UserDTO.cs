﻿using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string email { get; set; }

    }
}
