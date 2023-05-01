using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class ApplicationDTO
    {
        [Required]
        public int ApplicationId { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string JobTitle { get; set; }

        public string Salary { get; set; }
        public string Location { get; set; }
        public string Link { get; set; }
        public string Comments { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
