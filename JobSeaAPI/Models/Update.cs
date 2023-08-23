using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeaAPI.Models
{
    public class Update
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UpdateId { get; set; }
        public DateTime Created { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        [Required]
        public string Notes { get; set; }  
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
