using AutoMapper;
using Camino.Framework.Models;
using Camino.Service.Data.Identity;
using System.Security.Claims;
using Camino.IdentityManager.Models;
using Camino.Service.Data.Filters;

namespace Camino.Framework.Infrastructure.AutoMap
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<ApplicationUser, UserInfoModel>();
            CreateMap<ApplicationUser, UserResult>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed));
            CreateMap<UserResult, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.IsEmailConfirmed));
            CreateMap<ApplicationUser, UserIdentifierUpdateDto>();
            CreateMap<RoleResult, ApplicationRole>();
            CreateMap<UserRoleResult, ApplicationUserRole>();
            CreateMap<ApplicationUserClaim, UserClaimResult>();
            CreateMap<UserClaimResult, ApplicationUserClaim>();
            CreateMap<Claim, ClaimResult>();
            CreateMap<Service.Data.Identity.UserTokenResult, ApplicationUserToken>();
            CreateMap<ApplicationUserToken, Service.Data.Identity.UserTokenResult>();
            CreateMap<UserLoginDto, ApplicationUserLogin>();
            CreateMap<ApplicationUserLogin, UserLoginDto>();
            CreateMap<ApplicationRoleClaim, RoleClaimDto>();
            CreateMap<RoleClaimDto, ApplicationRoleClaim>();
            CreateMap<AuthorizationPolicyResult, ApplicationAuthorizationPolicy>();
            CreateMap<UserAuthorizationPolicyResult, ApplicationUserAuthorizationPolicy>();
            CreateMap<BaseFilter, BaseFilterModel>();
        }
    }
}
