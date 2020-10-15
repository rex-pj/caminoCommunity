using Camino.Service.Projections.Filters;
using System.Threading.Tasks;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Article;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticleBusiness
    {
        Task<int> CreateAsync(ArticleProjection category);
        ArticleProjection Find(long id);
        ArticleProjection FindDetail(long id);
        ArticleProjection FindByName(string name);
        Task<ArticleProjection> UpdateAsync(ArticleProjection article);
        Task<BasePageList<ArticleProjection>> GetAsync(ArticleFilter filter);
    }
}