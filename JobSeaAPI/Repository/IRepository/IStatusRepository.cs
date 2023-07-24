using JobSeaAPI.Models;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IStatusRepository
    {
        List<Status> GetStatuses();
    }
}
