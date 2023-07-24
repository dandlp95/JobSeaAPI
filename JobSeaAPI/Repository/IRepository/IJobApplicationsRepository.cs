using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IJobApplicationsRepository
    {
        Task<ApplicationDTO> CreateApplication(CreateApplicationDTO applicationDTORequest);
        Task DeleteApplication(int applicationId);
        Task UpdateApplication(Application application);
        Task<Application> GetApplication(string sqlQuery);
        List<Application> GetAllApplications(int userId);
    }
}
