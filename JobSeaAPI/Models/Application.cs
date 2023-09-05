using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeaAPI.Models
{
    public class Application
    {
        // Since everything should be sent to the user, I dont think I need an ApplicationDTO.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }
        [Required]
        public string Company { get; set; } 
        [Required]
        public string JobTitle { get; set; } 
        public int Salary { get; set; } 
        public string? Location { get; set; } 
        public string? Link { get; set; } 
        public string? Comments { get; set; } 
        [Required]
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? ModalityId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("ModalityId")]
        public Modality? Modality { get; set; }
    }
}
