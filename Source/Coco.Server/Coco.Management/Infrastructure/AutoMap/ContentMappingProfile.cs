using AutoMapper;
using Coco.Core.Entities.Content;
using Coco.Core.Dtos.General;
using Coco.Management.Models;
using Coco.Core.Dtos.Content;

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
            CreateMap<SetupViewModel, Setup>();
        }
    }
}
