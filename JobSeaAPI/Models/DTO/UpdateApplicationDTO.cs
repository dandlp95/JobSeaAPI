using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateApplicationDTO
    {
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public int? Salary { get; set; }
        public string Location { get; set; }
        public string Link { get; set; }
        public string? JobDetails { get; set; }
        public string Comments { get; set; }
        public int? ModalityId { get; set; }
    }
}
