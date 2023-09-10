using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using System.Text.Json;

namespace JobSeaAPI.Repository
{
    public class LocationRepository : ILocationRepository
    {
        public LocationRepository() 
        {
        }

        public List<City>? GetCities(int stateId)
        {
            List<City> cities = GetData<City>("city");
            return cities.Where(e => e.state_id == stateId).ToList();
        }

        public List<State> GetStates(int countryId)
        {
            List<State> states = GetData<State>("state");
            return states.Where(e => e.country_id == countryId).ToList();
        }

        public List<Country> GetCountries()
        {
            List<Country> countries = GetData<Country>();
            return countries.ToList();
        }

        private List<T>? GetData<T>(string? LocationEntityType = null) where T : class, new()
        {
            string path;
            if(LocationEntityType == "city")
            {
                path = "../JobSeaAPI/LocationData/cities.json";
            }
            else if(LocationEntityType == "state")
            {
                path = "../JobSeaAPI/LocationData/states.json";
            }
            else
            {
                path = "../JobSeaAPI/LocationData/countries.json";
            }
            List<T>? result = ReadDataFromFile<T>(path);  
            return result;
        }
        private List<T>? ReadDataFromFile<T>(string path) where T : class, new()
        {
            try
            {
                string jsonData = File.ReadAllText(path);
                List<T>? locations = JsonSerializer.Deserialize<List<T>?>(jsonData);
                return locations;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return null;
            }
        }
    }
}
