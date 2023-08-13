using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IJobApplicationsRepository
    {
        Task<ApplicationDTO> CreateApplication(CreateApplicationDTO applicationDTORequest);
        Task<bool> DeleteApplication(int applicationId);
        Task<Application> UpdateApplication(UpdateApplicationDTO application);
        Task<Application> GetApplication(string sqlQuery);
        Application GetApplication(int applicationId);
        List<Application> GetAllApplications(int userId);
    }
}
