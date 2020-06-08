using Api.Auth.Models;
using AutoMapper;
using Coco.Entities.Dtos;
using Coco.Entities.Dtos.General;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;

namespace Api.Auth.Infrastructure.MappingProfiles
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<UpdatePerItemModel, UpdatePerItemDto>();
            CreateMap<UpdatePerItemDto, UpdatePerItemModel>();
            CreateMap<UserFullDto, FullUserInfoModel>();
            CreateMap<ApplicationUser, FullUserInfoModel>();
            CreateMap<UserPhotoDto, UserAvatarModel>();
            CreateMap<UserPhotoDto, UserCoverModel>();
        }
    }
}
