using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword must match")]
        public string ConfirmPassword { get; set; }
    }
}
