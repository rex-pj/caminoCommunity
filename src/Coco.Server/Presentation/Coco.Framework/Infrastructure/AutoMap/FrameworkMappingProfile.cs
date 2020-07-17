using AutoMapper;
using Coco.Framework.Models;
using Coco.Business.Dtos.Identity;
using System.Security.Claims;

namespace Coco.Framework.Infrastructure.AutoMap
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<ApplicationUser, UserInfoModel>();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed));
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.IsEmailConfirmed));
            CreateMap<ApplicationUser, UserIdentifierUpdateDto>();
            CreateMap<RoleDto, ApplicationRole>();
            CreateMap<UserRoleDto, ApplicationUserRole>();
            CreateMap<ApplicationUserClaim, UserClaimDto>();
            CreateMap<UserClaimDto, ApplicationUserClaim>();
            CreateMap<Claim, ClaimDto>();
            CreateMap<UserTokenDto, ApplicationUserToken>();
            CreateMap<ApplicationUserToken, UserTokenDto>();
            CreateMap<UserLoginDto, ApplicationUserLogin>();
            CreateMap<ApplicationUserLogin, UserLoginDto>();
            CreateMap<ApplicationRoleClaim, RoleClaimDto>();
            CreateMap<RoleClaimDto, ApplicationRoleClaim>();
        }
    }
}
