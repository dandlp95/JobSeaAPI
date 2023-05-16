using JobSeaAPI.Models;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IJobApplicationsRepository 
    { 
        Task CreateApplication(Application application);
        Task DeleteApplication(Application application);
        Task UpdateApplication(Application application);
        Task<Application> GetApplication(string sqlQuery);
        Task<List<Application>> GetAllApplications();
    }
}
