using AutoMapper;
using Coco.Api.Framework.Models;
using Coco.Entities.Dtos.User;

namespace Coco.Api.Framework.MappingProfiles
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
        }
    }
}
