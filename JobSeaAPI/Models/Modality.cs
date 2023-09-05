using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models
{
    public class Modality
    {
        [Key]
        public int ModalityId { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
