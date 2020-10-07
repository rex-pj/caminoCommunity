using AutoMapper;
using Camino.DAL.Entities;
using Camino.Service.Projections.Content;

namespace Module.Api.Media.Infrastructure.AutoMap
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<UserPhoto, UserPhotoProjection>();
        }
    }
}
