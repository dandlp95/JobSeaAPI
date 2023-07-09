using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IJobApplicationsRepository
    { 
        void CreateApplication(Application application, Update update);
        Task DeleteApplication(Application application);
        Task UpdateApplication(Application application);
        Task<Application> GetApplication(string sqlQuery);
        List<Application> GetAllApplications(int userId);
        List<Update> GetAllUpdates(int userId, int applicationId);
    }
}
