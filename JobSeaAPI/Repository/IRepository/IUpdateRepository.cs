using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using System.Linq.Expressions;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IUpdateRepository
    {
        Task DeleteUpdate(Update update);
        Task DeleteUpdate(int updateId, int applicationId, int userId);
        Task DeleteUpdates(List<Update> updates);
        List<Update> GetUpdates(int userId, int applicationId);
        Task<Update> CreateUpdate(UpdateCreateDTO updateDTO, Application application);
        Task<Update> CreateUpdate(UpdateCreateDTO updateDTO);
        Update GetUpdate(int updateId);
        Task<Update> UpdateUpdate(UpdateUpdateDTO updateDTO, int updateId, int applicationId, int userId);
    }
}
