using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateDTO
    {
        public int UpdateId { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public string Notes { get; set; }
        // Having the Status here is helpful so client doesn't have to make another call for that information.
        public Status? Status { get; set; }
        public int ApplicationId { get; set; }
    }
}
