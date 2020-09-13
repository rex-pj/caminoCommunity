using AutoMapper;
using Camino.DAL.Entities;
using Camino.Service.Projections.Content;
using Module.Web.ArticleManagement.Models;

namespace Module.Web.ArticleManagement.Infrastructure.AutoMap
{
    public class ArticleMappingProfile : Profile
    {
        public ArticleMappingProfile()
        {
            CreateMap<ArticleModel, ArticleProjection>();
            CreateMap<ArticleProjection, ArticleModel>();
            CreateMap<ArticleProjection, Article>();
            CreateMap<Article, ArticleProjection>();

            CreateMap<ArticleCategoryModel, ArticleCategoryProjection>();
            CreateMap<ArticleCategoryProjection, ArticleCategoryModel>();
            CreateMap<ArticleCategoryProjection, ArticleCategory>();
            CreateMap<ArticleCategory, ArticleCategoryProjection>();
        }
    }
}
