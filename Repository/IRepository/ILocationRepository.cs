using JobSeaAPI.Models;

namespace JobSeaAPI.Repository.IRepository
{
    public interface ILocationRepository
    {
        List<City> GetCities(int stateId);
        List<State> GetStates(int countryId);
        List<Country> GetCountries();
    }
}
