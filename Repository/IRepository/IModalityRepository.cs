using JobSeaAPI.Models;

namespace JobSeaAPI.Repository.IRepository
{
    public interface IModalityRepository
    {
        List<Modality> GetModalities();
    }
}
