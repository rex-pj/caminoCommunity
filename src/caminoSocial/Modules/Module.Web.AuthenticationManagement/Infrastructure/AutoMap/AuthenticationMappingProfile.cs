using AutoMapper;
using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.Service.Data.Request;
using Module.Web.AuthenticationManagement.Models;

namespace Module.Web.SetupManagement.Infrastructure.AutoMap
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<UserFullProjection, UserModel>();
            CreateMap<UserProjection, UserModel>();
            CreateMap<UserFilterModel, UserFilter>();
        }
    }
}
