using AutoMapper;
using Coco.Entities.Domain.Content;
using Coco.Entities.Dtos;

namespace Api.Content.Infrastructure.AutoMap
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<UserPhoto, UserPhotoDto>();
        }
    }
}
