using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateUserDTO
    {
        [Required]
        public int UserId { get; set; }
        [StringLength(30, ErrorMessage = "Usernames cannot exceed 30 characters.")]
        public string Username { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
