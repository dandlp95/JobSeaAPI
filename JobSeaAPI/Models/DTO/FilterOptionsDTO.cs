namespace JobSeaAPI.Models.DTO
{
    public class FilterOptionsDTO
    {
        public string[]? Company { get; set; }
        public string[] Cities { get; set; }
        public string[] States { get; set; } 
        // use the modalitie's Ids
        public int[]? Modalities { get; set; }
        public int[]? StatusId { get; set; }
        public (int min, int max)? SalaryRange { get; set; }
    }
}
