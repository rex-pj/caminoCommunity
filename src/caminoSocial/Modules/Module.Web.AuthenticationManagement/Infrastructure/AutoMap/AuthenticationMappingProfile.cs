using AutoMapper;
using Camino.Business.Dtos.Identity;
using Module.Web.AuthenticationManagement.Models;

namespace Module.Web.SetupManagement.Infrastructure.AutoMap
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<UserFullDto, UserViewModel>();
            CreateMap<UserDto, UserViewModel>();
        }
    }
}
