using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateUpdateDTO
    {
        [Required(ErrorMessage = "Missing UpdateId.")]
        public int UpdateId { get; set; }
        public DateTime EventDate { get; set; }
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string notes { get; set; }
        public Status Status { get; set; }
        [Required(ErrorMessage = "Missing ApplicationId.")]
        public int ApplicationId { get; set; }
    }
}
