using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Services
{
    public class SqlBuilder : ISqlBuilder
    {
        public SqlBuilder() { }

        public string BuildSql(FilterOptionsDTO? filterOptions, string? searchTerm, int userId, int? skip, int? rows)
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
                    LEFT JOIN Modalities M ON M.ModalityId = A.ModalityId
                WHERE UserId = {userId}
                ";
            // FilterOptions should be passed as null in frontend to avoid checking each property.
            if (filterOptions is not null)
            {
                if (filterOptions?.States?.Length > 0)
                {
                    string statesFilter = string.Join(" OR ", filterOptions.States.Select(state => $"state = '{state}'"));
                    sqlQuery += $" AND ({statesFilter})";
                }
                if (filterOptions?.Cities?.Length > 0)
                {
                    string citiesFilter = string.Join(" OR ", filterOptions.Cities.Select(city => $"city = '{city}'"));
                    sqlQuery += $" AND ({citiesFilter})";
                }
                if (filterOptions?.Modalities?.Length > 0)
                {
                    string modalitiesFilter = string.Join(" OR ", filterOptions.Modalities.Select(modalityId => $"M.ModalityId = {modalityId}"));
                    sqlQuery += $" AND ({modalitiesFilter})";
                }
                if (filterOptions?.StatusId?.Length > 0)
                {
                    string statusFilter = string.Join(" OR ", filterOptions.StatusId.Select(statusId => $"S.StatusId = {statusId}"));
                    sqlQuery += $" AND ({statusFilter})";
                }

                if (filterOptions?.SalaryRange?.min is not null && filterOptions.SalaryRange?.max is not null)
                {
                    sqlQuery += $" AND Salary BETWEEN {filterOptions.SalaryRange?.min} AND {filterOptions.SalaryRange?.max}";
                }
                else if (filterOptions?.SalaryRange?.min is null && filterOptions?.SalaryRange?.max is not null)
                {
                    sqlQuery += $" AND Salary <= {filterOptions.SalaryRange?.max}";
                }
                else if (filterOptions?.SalaryRange?.min is not null && filterOptions.SalaryRange?.max is null)
                {
                    sqlQuery += $" AND Salary >= {filterOptions.SalaryRange?.min}";
                }

                if (filterOptions?.Company?.Length > 0)
                {
                    string companyFilter = string.Join(" OR ", filterOptions.Company.Select(company => $"company = '{company}'"));
                    sqlQuery += companyFilter;
                }
            }


            if (!string.IsNullOrEmpty(searchTerm))
            {
                string searchFilter = SearchTermSqlBuilder(searchTerm);
                sqlQuery += $" AND ({searchFilter})";
            }

            if (skip is not null && rows is not null)
            {
                sqlQuery += " ORDER BY A.ApplicationId OFFSET " + skip + " ROWS FETCH NEXT " + rows + " ROWS ONLY";
            }

            return sqlQuery;
        }

        private string SearchTermSqlBuilder(string searchTerm)
        {
            string[] words = searchTerm.Split(' ');
            string searchTermFilter = string.Join(" OR ", words.Select(term =>
                $"Company like '%{term}%' " + $"  OR JobTitle like '%{term}%'" + $"  OR State like '%{term}%'" + $"  OR City like '%{term}%'" + $"  OR Comments like '%{term}%'" + $"  OR M.Name like '%{term}%'" + $"  OR S.StatusName like '%{term}%'" + $"  OR U.Notes like '%{term}%'" + $"  OR A.JobDetails like '%{term}%'" +
                $"  OR EXISTS (SELECT TOP 1 1 FROM Updates U2 WHERE U2.ApplicationId = A.ApplicationId AND U2.Notes like '%{term}%')"
                ));


            return searchTermFilter;
        }


    }
}