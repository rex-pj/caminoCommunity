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
        Task<ArticleCategoryResult> FindAsync(IdRequestFilter<int> filter);
        Task<BasePageList<ArticleCategoryResult>> GetAsync(ArticleCategoryFilter filter);
        IList<ArticleCategoryResult> Search(IdRequestFilter<int?> idRequestFilter, BaseFilter filter);
        IList<ArticleCategoryResult> SearchParents(IdRequestFilter<int?> idRequestFilter, BaseFilter filter);
        Task<int> CreateAsync(ArticleCategoryModifyRequest category);
        Task<bool> UpdateAsync(ArticleCategoryModifyRequest category);
        Task<ArticleCategoryResult> FindByNameAsync(string name);
        Task<bool> DeactivateAsync(ArticleCategoryModifyRequest request);
        Task<bool> ActiveAsync(ArticleCategoryModifyRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
