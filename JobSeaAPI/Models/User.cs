using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeaAPI.Models
{
    [Index(nameof(email), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }    
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Not sure if this would work...
    }
}
