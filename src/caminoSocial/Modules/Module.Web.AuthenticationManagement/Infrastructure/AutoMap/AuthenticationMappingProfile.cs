using AutoMapper;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.Service.Projections.Request;
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
