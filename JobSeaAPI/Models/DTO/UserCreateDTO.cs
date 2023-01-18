using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UserCreateDTO
    {
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public string email { get; set; }
    }
}
