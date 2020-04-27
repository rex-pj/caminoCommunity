using AutoMapper;
using Coco.Entities.Domain.Content;
using Coco.Entities.Dtos.Content;
using Coco.Management.Models;

namespace Coco.Management.MappingProfiles
{
    public class ArticleCategoryMappingProfile : Profile
    {
        public ArticleCategoryMappingProfile()
        {
            CreateMap<ArticleCategoryViewModel, ArticleCategoryDto>();
            CreateMap<ArticleCategoryDto, ArticleCategoryViewModel>();
            CreateMap<ArticleCategoryDto, ArticleCategory>();
            CreateMap<ArticleCategory, ArticleCategoryDto>();
        }
    }
}
