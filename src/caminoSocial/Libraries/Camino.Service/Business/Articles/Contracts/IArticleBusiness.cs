using Camino.Service.Projections.Filters;
using System.Threading.Tasks;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Article;
using System.Collections.Generic;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticleBusiness
    {
        Task<int> CreateAsync(ArticleProjection category);
        Task<ArticleProjection> FindAsync(long id);
        Task<ArticleProjection> FindDetailAsync(long id);
        ArticleProjection FindByName(string name);
        Task<ArticleProjection> UpdateAsync(ArticleProjection article);
        Task<BasePageList<ArticleProjection>> GetAsync(ArticleFilter filter);
        Task<IList<ArticleProjection>> GetRelevantsAsync(long id, ArticleFilter filter);
    }
}