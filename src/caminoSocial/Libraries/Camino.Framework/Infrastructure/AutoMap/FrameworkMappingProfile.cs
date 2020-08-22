using AutoMapper;
using Camino.Framework.Models;
using Camino.Service.Data.Identity;
using System.Security.Claims;
using Camino.IdentityManager.Models;
using Camino.Service.Data.Filters;
using Camino.Service.Data.Request;

namespace Camino.Framework.Infrastructure.AutoMap
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<ApplicationUser, UserInfoModel>();
            CreateMap<ApplicationUser, UserProjection>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed));
            CreateMap<UserProjection, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.IsEmailConfirmed));
            CreateMap<ApplicationUser, UserIdentifierUpdateRequest>();
            CreateMap<RoleProjection, ApplicationRole>();
            CreateMap<UserRoleProjection, ApplicationUserRole>();
            CreateMap<ApplicationUserClaim, UserClaimProjection>();
            CreateMap<UserClaimProjection, ApplicationUserClaim>();
            CreateMap<Claim, ClaimProjection>();
            CreateMap<UserTokenProjection, ApplicationUserToken>();
            CreateMap<ApplicationUserToken, UserTokenProjection>();
            CreateMap<UserLoginRequest, ApplicationUserLogin>();
            CreateMap<ApplicationUserLogin, UserLoginRequest>();
            CreateMap<ApplicationRoleClaim, RoleClaimProjection>();
            CreateMap<RoleClaimProjection, ApplicationRoleClaim>();
            CreateMap<AuthorizationPolicyProjection, ApplicationAuthorizationPolicy>();
            CreateMap<UserAuthorizationPolicyProjection, ApplicationUserAuthorizationPolicy>();
            CreateMap<BaseFilter, BaseFilterModel>();
        }
    }
}
