using AutoMapper;
using PredicaTask.Application.Dtos.UserDtos;
using PredicaTask.Domain;

namespace PredicaTask.Application.Profiles.UserProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
        }
    }
}