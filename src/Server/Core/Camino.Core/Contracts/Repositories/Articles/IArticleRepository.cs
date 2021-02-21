using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using System.Collections.Generic;
using Camino.Shared.Requests.Articles;

namespace Camino.Core.Contracts.Repositories.Articles
{
    public interface IArticleRepository
    {
        Task<int> CreateAsync(ArticleModifyRequest request);
        Task<ArticleResult> FindAsync(long id);
        Task<ArticleResult> FindDetailAsync(long id);
        ArticleResult FindByName(string name);
        Task<bool> UpdateAsync(ArticleModifyRequest request);
        Task<BasePageList<ArticleResult>> GetAsync(ArticleFilter filter);
        Task<IList<ArticleResult>> GetRelevantsAsync(long id, ArticleFilter filter);
        Task<bool> DeleteAsync(long id);
        Task<bool> SoftDeleteAsync(long id);
    }
}