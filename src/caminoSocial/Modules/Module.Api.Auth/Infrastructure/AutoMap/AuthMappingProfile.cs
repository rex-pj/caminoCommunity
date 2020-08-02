using Module.Api.Auth.Models;
using AutoMapper;
using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using Camino.Framework.Models;
using Camino.Business.Dtos.Content;
using Camino.Data.Enums;
using Camino.IdentityManager.Models;

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
