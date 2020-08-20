using AutoMapper;
using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Module.Web.AuthenticationManagement.Models;

namespace Module.Web.SetupManagement.Infrastructure.AutoMap
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<UserFullDto, UserModel>();
            CreateMap<UserResult, UserModel>();
            CreateMap<UserFilterModel, UserFilter>();
        }
    }
}
