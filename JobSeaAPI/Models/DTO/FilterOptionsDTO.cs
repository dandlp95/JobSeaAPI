namespace JobSeaAPI.Models.DTO
{
    public class FilterOptionsDTO
    {
        public (string city, string state)[]? Locations { get; set; }
        public int[]? Modalities { get; set; }
        public int? StatusId { get; set; }
        public (int min, int max)? SalaryRange { get; set; }
    }
}
