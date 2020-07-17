using AutoMapper;
using Camino.Data.Entities.Content;
using Camino.Business.Dtos.General;
using Camino.Management.Models;
using Camino.Business.Dtos.Content;

namespace Camino.Management.Infrastructure.AutoMap
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<ArticleCategoryViewModel, ArticleCategoryDto>();
            CreateMap<ArticleCategoryDto, ArticleCategoryViewModel>();
            CreateMap<ArticleCategoryDto, ArticleCategory>();
            CreateMap<ArticleCategory, ArticleCategoryDto>();
            CreateMap<SetupViewModel, SetupDto>();
        }
    }
}
