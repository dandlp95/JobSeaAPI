using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IJobApplicationsRepository
    {
        Application CreateApplication(CreateApplicationDTO applicationDTO);
        Task DeleteApplication(Application application);
        Task UpdateApplication(Application application);
        Task<Application> GetApplication(string sqlQuery);
        List<Application> GetAllApplications(int userId);
        List<Update> GetAllUpdates(int userId, int applicationId);
    }
}
