using AutoMapper;
using Camino.Data.Entities.Content;
using Camino.Business.Dtos.Content;

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
