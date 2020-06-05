using AutoMapper;
using Coco.Entities.Domain.Content;
using Coco.Entities.Dtos;

namespace Api.Content.Infrastructure.MappingProfiles
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<UserPhoto, UserPhotoDto>();
        }
    }
}
