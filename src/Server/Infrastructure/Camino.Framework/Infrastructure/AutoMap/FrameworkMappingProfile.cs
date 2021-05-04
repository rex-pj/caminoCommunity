using AutoMapper;
using Camino.Shared.Results.Identifiers;
using Camino.Core.Domain.Identities;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Requests.Identifiers;
using Camino.Shared.Requests.Authorization;

namespace Camino.Framework.Infrastructure.AutoMap
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<UserResult, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.IsEmailConfirmed));

            CreateMap<ApplicationUser, UserModifyRequest>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed));

            CreateMap<RoleResult, ApplicationRole>();

            CreateMap<ApplicationRole, RoleModifyRequest>();
        }
    }
}
