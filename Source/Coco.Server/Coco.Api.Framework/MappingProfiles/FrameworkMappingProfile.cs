using AutoMapper;
using Coco.Api.Framework.Models;
using Coco.Entities.Model.User;

namespace Coco.Api.Framework.MappingProfiles
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<ApplicationUser, UserInfoModel>();
            CreateMap<ApplicationUser, UserModel>();
            CreateMap<UserFullModel, UserInfoExt>();
            CreateMap<UserModel, ApplicationUser>();
            CreateMap<UserLoggedInModel, ApplicationUser>();
            CreateMap<ApplicationUser, UserProfileUpdateModel>();
        }
    }
}
