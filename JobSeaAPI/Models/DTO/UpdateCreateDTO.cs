using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateCreateDTO
    {
        [Column(TypeName = "date")]
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public string notes { get; set; }
        [Required(ErrorMessage = "Status Id is required")]
        public int StatusId { get; set; }
    }
}
