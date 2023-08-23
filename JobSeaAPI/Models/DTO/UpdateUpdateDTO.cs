using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateUpdateDTO
    {
        [Column(TypeName = "date")]
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string notes { get; set; }
        public int StatusId { get; set; }
    }
}
