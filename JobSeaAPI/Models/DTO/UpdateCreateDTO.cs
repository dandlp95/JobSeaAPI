using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateCreateDTO
    {
        public DateTime EventDate { get; set; }
        public string notes { get; set; }
        [Required(ErrorMessage = "Status Id is required")]
        public int StatusId { get; set; }
        // This will be manually checked in the create update controller instead of using a Require decorator.
        public int ApplicationId { get; set; } 
    }
}
