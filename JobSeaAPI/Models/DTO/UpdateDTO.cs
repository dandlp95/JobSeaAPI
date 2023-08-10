using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateDTO
    {
        public int UpdateId { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime EventDate { get; set; }
        public string notes { get; set; }
        // Having the Status here is helpful so client doesn't have to make another call for that information.
        public Status Status { get; set; }
        public int ApplicationId { get; set; }
    }
}
