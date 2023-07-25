using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IJobApplicationsRepository
    {
        Task<ApplicationDTO> CreateApplication(CreateApplicationDTO applicationDTORequest);
        Task<bool> DeleteApplication(int applicationId);
        Task UpdateApplication(Application application);
        Task<Application> GetApplication(string sqlQuery);
        public Application GetApplication(int applicationId);
        List<Application> GetAllApplications(int userId);
    }
}
