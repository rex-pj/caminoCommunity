using Api.Auth.Models;
using AutoMapper;
using Coco.Core.Dtos.General;
using Coco.Core.Dtos.Identity;
using Coco.Framework.Models;
using Coco.Core.Dtos.Content;
using Coco.Core.Entities.Enums;

namespace Api.Auth.Infrastructure.AutoMap
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<UpdatePerItemModel, UpdatePerItem>();
            CreateMap<UpdatePerItem, UpdatePerItemModel>();
            CreateMap<UserFullDto, FullUserInfoModel>();
            CreateMap<ApplicationUser, FullUserInfoModel>();
            CreateMap<UserPhotoDto, UserAvatarModel>();
            CreateMap<UserPhotoDto, UserPhotoModel>()
                .ForMember(dest => dest.PhotoType, opt => opt.MapFrom((src, dest) => { 
                    if(src.TypeId > 0)
                    {
                        return (UserPhotoType)src.TypeId;
                    }
                    return UserPhotoType.Undefined;
                }));
        }
    }
}
