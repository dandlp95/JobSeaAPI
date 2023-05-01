using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateDTO
    {
        public int UpdateId { get; set; }
        public DateTime Created { get; set; }
        public DateTime EventDate { get; set; }
        public string notes { get; set; }
        public int StatusId { get; set; }
        public int ApplicationId { get; set; }
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
    }
}
