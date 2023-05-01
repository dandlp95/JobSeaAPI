using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeaAPI.Models
{
    public class Update
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UpdateId { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime EventDate { get; set; }
        [Required]
        public string notes { get; set; }  
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int ApplicationId { get; set; }
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
    }
}
