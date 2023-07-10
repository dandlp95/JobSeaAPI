using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeaAPI.Models.DTO
{
    public class UpdateCreateDTO
    {
        public int UpdateId { get; set; }
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
