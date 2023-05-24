using System.ComponentModel.DataAnnotations;
// Mix between application and update dto, because an update also has to be created 
// the first time as well.
namespace JobSeaAPI.Models.DTO
{
    public class CreateApplicationDTO
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
        public DateTime EventDate { get; set; }
        public string notes { get; set; }
        public int StatusId { get; set; }

    }
}
