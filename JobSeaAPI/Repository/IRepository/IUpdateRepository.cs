using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using System.Linq.Expressions;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IUpdateRepository
    {
        Task<bool> DeleteUpdate(List<Update> updates);
        List<Update> GetUpdates(int userId, int applicationId);
        public Task<Update> CreateUpdate(UpdateCreateDTO updateDTO, Application application);
        public Task<Update> CreateUpdate(UpdateCreateDTO updateDTO, int applicationId);
    }
}
