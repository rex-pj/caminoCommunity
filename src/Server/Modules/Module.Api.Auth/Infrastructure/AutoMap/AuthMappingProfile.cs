using Module.Api.Auth.Models;
using AutoMapper;
using Camino.Framework.Models;
using Camino.Data.Enums;
using Camino.Service.Projections.Request;
using Camino.Service.Projections.Media;

namespace Module.Api.Auth.Infrastructure.AutoMap
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<UpdatePerItemModel, UpdateItemRequest>();
            CreateMap<UpdateItemRequest, UpdatePerItemModel>();
            CreateMap<UserPhotoProjection, UserAvatarModel>();
            CreateMap<UserPhotoProjection, UserPhotoModel>()
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
