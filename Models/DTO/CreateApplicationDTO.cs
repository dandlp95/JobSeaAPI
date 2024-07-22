using System.ComponentModel.DataAnnotations;
// Mix between application and update dto, because an update also has to be created 
// the first time as well.
namespace JobSeaAPI.Models.DTO
{
    public class CreateApplicationDTO
    {
        [Required(ErrorMessage = "Name of company is required.")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Job title is required")]
        public string JobTitle { get; set; }
        public int? Salary { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string Link { get; set; }
        public string? JobDetails { get; set; }
        public string Comments { get; set; }
        public int? ModalityId { get; set; }
        [Required]
        public UpdateCreateDTO FirstUpdate { get; set; }
    }
}
