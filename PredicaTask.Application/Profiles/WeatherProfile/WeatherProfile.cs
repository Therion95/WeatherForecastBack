using AutoMapper;
using PredicaTask.Application.Dtos.WeatherDtos;
using PredicaTask.Domain;

namespace PredicaTask.Application.Profiles.WeatherProfile
{
    public class WeatherProfile : Profile
    {
        public WeatherProfile()
        {
            CreateMap<Weather, WeatherDto>().ReverseMap();
            CreateMap<Weather, CreateWeatherDto>().ReverseMap();
            CreateMap<Weather, UpdateWeatherDto>().ReverseMap();
        }
    }
}