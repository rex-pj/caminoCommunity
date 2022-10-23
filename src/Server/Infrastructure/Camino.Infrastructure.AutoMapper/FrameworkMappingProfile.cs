using AutoMapper;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Infrastructure.Identity.Core;

namespace Camino.Infrastructure.AutoMapper
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
