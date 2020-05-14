using AutoMapper;
using Coco.Framework.Models;
using Coco.Entities.Dtos.User;
using Coco.Entities.Dtos.Auth;

namespace Coco.Framework.Infrastructure.MappingProfiles
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<ApplicationUser, UserInfoModel>();
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UserFullDto, FullUserInfoModel>();
            CreateMap<UserDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserIdentifierUpdateDto>();
            CreateMap<RoleDto, ApplicationRole>();
            CreateMap<UserRoleDto, ApplicationUserRole>();
        }
    }
}
