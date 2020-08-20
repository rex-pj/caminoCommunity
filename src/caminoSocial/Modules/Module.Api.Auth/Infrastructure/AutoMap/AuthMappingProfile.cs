using Module.Api.Auth.Models;
using AutoMapper;
using Camino.Service.Data.Common;
using Camino.Service.Data.Identity;
using Camino.Framework.Models;
using Camino.Service.Data.Content;
using Camino.Data.Enums;
using Camino.IdentityManager.Models;
using Camino.Service.Data.Request;

namespace  Module.Api.Auth.Infrastructure.AutoMap
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<UpdatePerItemModel, UpdateItemRequest>();
            CreateMap<UpdateItemRequest, UpdatePerItemModel>();
            CreateMap<UserFullDto, FullUserInfoModel>();
            CreateMap<ApplicationUser, FullUserInfoModel>();
            CreateMap<UserPhotoResult, UserAvatarModel>();
            CreateMap<UserPhotoResult, UserPhotoModel>()
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
