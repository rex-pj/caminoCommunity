using AutoMapper;
using Camino.Shared.Results.Identifiers;
using Camino.Core.Domain.Identities;
using Camino.Shared.Results.Authorization;

namespace Camino.Framework.Infrastructure.AutoMap
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<ApplicationUser, UserResult>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed));
            CreateMap<UserResult, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.IsEmailConfirmed));
            CreateMap<RoleResult, ApplicationRole>();
            CreateMap<ApplicationRole, RoleResult>();
        }
    }
}
