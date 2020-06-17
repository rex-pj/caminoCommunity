using AutoMapper;
using Coco.Entities.Domain.Content;
using Coco.Entities.Dtos.Content;
using Coco.Management.Models;

namespace Coco.Management.Infrastructure.AutoMap
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<ArticleCategoryViewModel, ArticleCategoryDto>();
            CreateMap<ArticleCategoryDto, ArticleCategoryViewModel>();
            CreateMap<ArticleCategoryDto, ArticleCategory>();
            CreateMap<ArticleCategory, ArticleCategoryDto>();
        }
    }
}
