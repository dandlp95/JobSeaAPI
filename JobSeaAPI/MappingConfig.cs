using AutoMapper;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;

namespace JobSeaAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Application, ApplicationDTO>().ReverseMap();
            CreateMap<Status, StatusDTO>().ReverseMap();
        }

    }
}
