using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using System.Threading.Tasks;
using Camino.Service.Data.PageList;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticleBusiness
    {
        int Add(ArticleProjection category);
        ArticleProjection Find(long id);
        ArticleProjection FindDetail(long id);
        ArticleProjection FindByName(string name);
        ArticleProjection Update(ArticleProjection article);
        Task<BasePageList<ArticleProjection>> GetAsync(ArticleFilter filter);
    }
}