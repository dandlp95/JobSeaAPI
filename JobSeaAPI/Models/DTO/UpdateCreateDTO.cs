using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateCreateDTO
    {
        public DateTime? EventDate { get; set; }
        public string notes { get; set; }
        [Required(ErrorMessage = "Status Id is required")]
        public int StatusId { get; set; }
    }
}
