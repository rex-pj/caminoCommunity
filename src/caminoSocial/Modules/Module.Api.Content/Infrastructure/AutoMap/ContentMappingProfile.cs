using AutoMapper;
using Camino.DAL.Entities;
using Camino.Service.Data.Content;

namespace Module.Api.Content.Infrastructure.AutoMap
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<UserPhoto, UserPhotoResult>();
        }
    }
}
