using AutoMapper;
using Coco.Data.Entities.Content;
using Coco.Business.Dtos.General;
using Coco.Management.Models;
using Coco.Business.Dtos.Content;

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
            CreateMap<SetupViewModel, SetupDto>();
        }
    }
}
