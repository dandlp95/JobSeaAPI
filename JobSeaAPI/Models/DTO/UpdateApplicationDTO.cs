using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateApplicationDTO
    {
        [Required]
        public int ApplicationId { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public int Salary { get; set; }
        public string Location { get; set; }
        public string Link { get; set; }
        public string Comments { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public int UserId { get; set; }
    }
}
