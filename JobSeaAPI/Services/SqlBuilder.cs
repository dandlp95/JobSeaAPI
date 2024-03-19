using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Services
{
    public class SqlBuilder : ISqlBuilder
    {
        public SqlBuilder() { }

        public string BuildSql(FilterOptionsDTO filterOptions)
        {
            string sqlQuery = @"
                SELECT ApplicationId, Company, JobTitle, Salary, City, State, Link, JobDetails, Comments, Created,
                    Last Updated, UserId, ModalityId
                FROM Application     
                WHERE 1 = 1
                ";

            if(filterOptions.Locations is not null)
            { 
                string locationsFilter = string.Join(" OR ", filterOptions.Locations.Select(location => $"(city = '{location.city}' AND state = '{location.state}')"));
                sqlQuery += $" AND ({locationsFilter})";
            }
            if(filterOptions.Modalities is not null) 
            {
                string modalitiesFilter = string.Join(" OR ", filterOptions.Modalities.Select(modalityId => $"ModalityId = {modalityId}"));
                sqlQuery += $" AND ({modalitiesFilter})";
            }
            if(filterOptions.StatusId is not null)
            {
                sqlQuery += $"AND StatusId = {filterOptions.StatusId}";
            }
            if(filterOptions.SalaryRange is not null)
            {
                sqlQuery += $"AND Salary BETWEEN ${filterOptions.SalaryRange?.min} AND ${filterOptions.SalaryRange?.max}";
            }

            return sqlQuery;
        }
    }
}