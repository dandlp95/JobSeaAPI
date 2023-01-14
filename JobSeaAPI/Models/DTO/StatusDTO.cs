using System.ComponentModel.DataAnnotations;

namespace JobSeaAPI.Models.DTO
{
    public class StatusDTO
    {
        [Required]
        public int StatusId { get; set; }
        [Required]
        public string CurrStatus { get; set; }
        public string Notes { get; set; }
        [Required]
        public int ApplicationId { get; set; }

    }
}
