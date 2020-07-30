using AutoMapper;
using Camino.Business.Dtos.Content;
using Camino.Data.Entities.Content;
using Module.Web.ArticleManagement.Models;

namespace Module.Web.ArticleManagement.Infrastructure.AutoMap
{
    public class ArticleMappingProfile : Profile
    {
        public ArticleMappingProfile()
        {
            CreateMap<ArticleViewModel, ArticleDto>();
            CreateMap<ArticleDto, ArticleViewModel>();
            CreateMap<ArticleDto, Article>();
            CreateMap<Article, ArticleDto>();

            CreateMap<ArticleCategoryViewModel, ArticleCategoryDto>();
            CreateMap<ArticleCategoryDto, ArticleCategoryViewModel>();
            CreateMap<ArticleCategoryDto, ArticleCategory>();
            CreateMap<ArticleCategory, ArticleCategoryDto>();
        }
    }
}
