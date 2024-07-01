using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Services
{
    public interface ISqlBuilder
    {
        public string BuildSql(FilterOptionsDTO filterOptions, string? searchTerm, int userId);
    }
}
