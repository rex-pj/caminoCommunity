using AutoMapper;
using Camino.DAL.Entities;
using Camino.Service.Projections.Content;

namespace Module.Api.Content.Infrastructure.AutoMap
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<UserPhoto, UserPhotoProjection>();
        }
    }
}
