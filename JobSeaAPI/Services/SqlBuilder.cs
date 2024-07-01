using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Services
{
    public class SqlBuilder : ISqlBuilder
    {
        public SqlBuilder() { }

        public string BuildSql(FilterOptionsDTO filterOptions, string? searchTerm, int userId)
        {
            string sqlQuery = @$"
                SELECT A.ApplicationId, A.Company, A.JobTitle, A.Salary, A.City, A.State, A.Link, A.JobDetails, A.Comments, A.Created,
                    A.LastUpdated, A.UserId, A.ModalityId, S.StatusName
                FROM Applications A
                    JOIN (
                        SELECT U.ApplicationId, MAX(UpdateId) AS MaxId
                        FROM Updates U
                        GROUP BY U.ApplicationId
                    ) latest_u ON A.ApplicationId = latest_u.ApplicationId
                    JOIN Updates U ON latest_u.MaxId = U.UpdateId
                    LEFT JOIN Status S ON S.StatusId = U.StatusId
                    LEFT JOIN Modality M ON M.Id = A.ModalityId
                WHERE UserId = {userId}
                ";

            if (filterOptions.States is not null)
            {
                string statesFilter = string.Join(" OR ", filterOptions.States.Select(state => $"state = '{state}')"));
                sqlQuery += $" AND ({statesFilter})";
            }
            if (filterOptions.Cities is not null)
            {
                string citiesFilter = string.Join(" OR ", filterOptions.Cities.Select(city => $"city = '{city}'"));
                sqlQuery += $" AND ({citiesFilter})";
            }
            if (filterOptions.Modalities is not null)
            {
                string modalitiesFilter = string.Join(" OR ", filterOptions.Modalities.Select(modalityId => $"ModalityId = {modalityId}"));
                sqlQuery += $" AND ({modalitiesFilter})";
            }
            if (filterOptions.StatusId is not null)
            {
                string statusFilter = string.Join(" OR ", filterOptions.StatusId.Select(statusId => $"StatusId = {statusId}"));
                sqlQuery += $"AND {statusFilter}";
            }
            //Need to update this in case one or both salary range numbers are null...
            if (filterOptions.SalaryRange is not null)
            {
                sqlQuery += $"AND Salary BETWEEN ${filterOptions.SalaryRange?.min} AND ${filterOptions.SalaryRange?.max}";
            }
            if (filterOptions.Company is not null)
            {
                string companyFilter = string.Join(" OR ", filterOptions.Company.Select(company => $"company = '{company}'"));
                sqlQuery += companyFilter;
            }

            if(searchTerm is not null)
            {
                string searchFilter = SearchTermSqlBuilder(searchTerm);
                sqlQuery += $" OR ({searchFilter})";
            }

            return sqlQuery;
        }

        private string SearchTermSqlBuilder(string searchTerm)
        {
            string[] words = searchTerm.Split(' ');
            string searchTermFilter = string.Join(" OR ", words.Select(term =>
                $"Company like '%{term}%' " + $"  OR JobTitle like '%{term}%'" + $"  OR State like '%{term}%'" + $"  OR City like '%{term}%'" + $"  OR Comments like '%{term}%'" + $"  OR M.Name like '%{term}%'" + $"  OR S.Name like '%{term}%'" + $"  OR U.Notes like '%{term}%'" + $""
                ));


            return searchTermFilter;
        }


    }
}