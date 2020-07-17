using AutoMapper;
using Coco.Core.Entities.Content;
using Coco.Core.Dtos.Content;

namespace Coco.Api.Content.Infrastructure.AutoMap
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<UserPhoto, UserPhotoDto>();
        }
    }
}
