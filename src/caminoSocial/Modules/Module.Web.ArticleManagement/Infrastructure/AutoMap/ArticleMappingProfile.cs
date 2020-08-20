using AutoMapper;
using Camino.DAL.Entities;
using Camino.Service.Data.Content;
using Module.Web.ArticleManagement.Models;

namespace Module.Web.ArticleManagement.Infrastructure.AutoMap
{
    public class ArticleMappingProfile : Profile
    {
        public ArticleMappingProfile()
        {
            CreateMap<ArticleModel, ArticleResult>();
            CreateMap<ArticleResult, ArticleModel>();
            CreateMap<ArticleResult, Article>();
            CreateMap<Article, ArticleResult>();

            CreateMap<ArticleCategoryModel, ArticleCategoryResult>();
            CreateMap<ArticleCategoryResult, ArticleCategoryModel>();
            CreateMap<ArticleCategoryResult, ArticleCategory>();
            CreateMap<ArticleCategory, ArticleCategoryResult>();
        }
    }
}
