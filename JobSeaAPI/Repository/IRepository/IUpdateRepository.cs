using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using System.Linq.Expressions;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IUpdateRepository
    {
        Task<bool> DeleteUpdate(Update update);
        Task<bool> DeleteUpdate(int updateId);
        Task<bool> DeleteUpdates(List<Update> updates);
        List<Update> GetUpdates(int userId, int applicationId);
        public Task<Update> CreateUpdate(UpdateCreateDTO updateDTO, Application application);
        public Task<Update> CreateUpdate(UpdateDTO updateDTO);
        public Update GetUpdate(int updateId);
    }
}
