using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateUpdateDTO
    {
        [Required(ErrorMessage = "Missing UpdateId.")]
        public int UpdateId { get; set; }
        public DateTime EventDate { get; set; }
        public string notes { get; set; }
        public Status Status { get; set; }
        [Required(ErrorMessage = "Missing ApplicationId.")]
        public int ApplicationId { get; set; }
    }
}
