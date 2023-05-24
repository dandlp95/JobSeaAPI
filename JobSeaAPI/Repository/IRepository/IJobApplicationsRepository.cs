using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IJobApplicationsRepository : IRepository<Application>
    { 
        Task CreateApplication(ApplicationDTO application, UpdateDTO update);
        Task DeleteApplication(Application application);
        Task UpdateApplication(Application application);
        Task<Application> GetApplication(string sqlQuery);
        Task<List<Application>> GetAllApplications();
    }
}
