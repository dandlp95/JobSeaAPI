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
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<Application, ApplicationCreateDTO>().ReverseMap();
            CreateMap<Update, UpdateDTO>().ReverseMap();
            CreateMap<Application, CreateApplicationDTO>().ReverseMap();
            CreateMap<Update, CreateApplicationDTO>().ReverseMap();
        }

    }
}
