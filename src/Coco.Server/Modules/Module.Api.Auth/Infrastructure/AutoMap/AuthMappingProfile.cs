using Module.Api.Auth.Models;
using AutoMapper;
using Coco.Business.Dtos.General;
using Coco.Business.Dtos.Identity;
using Coco.Framework.Models;
using Coco.Business.Dtos.Content;
using Coco.Data.Enums;

namespace  Module.Api.Auth.Infrastructure.AutoMap
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
                        return (UserPhotoKind)src.TypeId;
                    }
                    return UserPhotoKind.Undefined;
                }));
        }
    }
}
