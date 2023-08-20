using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IJobApplicationsRepository
    {
        Task<Application> CreateApplication(CreateApplicationDTO applicationDTORequest, int userId);
        Task DeleteApplication(int applicationId, int userId);
        Task<Application> UpdateApplication(UpdateApplicationDTO application, int applicationId, int userId, bool updateAllFields = false);
        Task<Application> GetApplication(string sqlQuery);
        Application GetApplication(int applicationId, int userId);
        List<Application> GetAllApplications(int userId);
    }
}
