using Api.Auth.Models;
using AutoMapper;
using Coco.Entities.Dtos;
using Coco.Entities.Dtos.General;
using Coco.Entities.Dtos.User;
using Coco.Entities.Enums;
using Coco.Framework.Models;

namespace Api.Auth.Infrastructure.AutoMap
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
            CreateMap<UserPhotoDto, UserPhotoModel>()
                .ForMember(dest => dest.PhotoType, opt => opt.MapFrom((src, dest) => { 
                    if(src.TypeId > 0)
                    {
                        return (UserPhotoTypeEnum)src.TypeId;
                    }
                    return UserPhotoTypeEnum.Undefined;
                }));
        }
    }
}
