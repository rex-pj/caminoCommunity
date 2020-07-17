using AutoMapper;
using Coco.Data.Entities.Content;
using Coco.Business.Dtos.Content;

namespace Module.Api.Content.Infrastructure.AutoMap
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<UserPhoto, UserPhotoDto>();
        }
    }
}
