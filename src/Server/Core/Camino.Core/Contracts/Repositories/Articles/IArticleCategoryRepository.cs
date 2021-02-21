using Camino.Shared.Requests.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using Camino.Shared.Requests.Articles;

namespace Camino.Core.Contracts.Repositories.Articles
{
    public interface IArticleCategoryRepository
    {
        Task<ArticleCategoryResult> FindAsync(long id);
        Task<BasePageList<ArticleCategoryResult>> GetAsync(ArticleCategoryFilter filter);
        IList<ArticleCategoryResult> Search(string search = "", long? currentId = null, int page = 1, int pageSize = 10);
        IList<ArticleCategoryResult> SearchParents(string search = "", long? currentId = null, int page = 1, int pageSize = 10);
        Task<int> CreateAsync(ArticleCategoryModifyRequest category);
        Task<bool> UpdateAsync(ArticleCategoryModifyRequest category);
        Task<ArticleCategoryResult> FindByNameAsync(string name);
    }
}
