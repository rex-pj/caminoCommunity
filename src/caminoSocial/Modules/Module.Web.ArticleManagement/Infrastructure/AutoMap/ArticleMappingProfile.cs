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
            CreateMap<ArticleModel, ArticleDto>();
            CreateMap<ArticleDto, ArticleModel>();
            CreateMap<ArticleDto, Article>();
            CreateMap<Article, ArticleDto>();

            CreateMap<ArticleCategoryModel, ArticleCategoryDto>();
            CreateMap<ArticleCategoryDto, ArticleCategoryModel>();
            CreateMap<ArticleCategoryDto, ArticleCategory>();
            CreateMap<ArticleCategory, ArticleCategoryDto>();
        }
    }
}
