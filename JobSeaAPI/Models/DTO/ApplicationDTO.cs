using System.ComponentModel.DataAnnotations;

// This DTO is used for updates to the application or get any other data
// from the client related to an Application entity
namespace JobSeaAPI.Models.DTO
{
    public class ApplicationDTO
    {
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
