using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class ApplicationDTO
    {
        public int ApplicationId { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public int? Salary { get; set; }
        public string? Location { get; set; }
        public string? Link { get; set; }
        public string? Comments { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public int UserId { get; set; }
        public int? ModalityId { get; set; }
    }
}
