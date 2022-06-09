using Camino.Application.Contracts.AppServices.Articles.Dtos;

namespace Camino.Application.Contracts.AppServices.Articles
{
    public interface IArticleCategoryAppService
    {
        Task<ArticleCategoryResult> FindAsync(IdRequestFilter<int> filter);
        Task<BasePageList<ArticleCategoryResult>> GetAsync(ArticleCategoryFilter filter);
        IList<ArticleCategoryResult> Search(IdRequestFilter<int?> idRequestFilter, BaseFilter filter);
        IList<ArticleCategoryResult> SearchParents(IdRequestFilter<int?> idRequestFilter, BaseFilter filter);
        Task<int> CreateAsync(ArticleCategoryModifyRequest category);
        Task<bool> UpdateAsync(ArticleCategoryModifyRequest category);
        Task<ArticleCategoryResult> FindByNameAsync(string name);
        Task<bool> ActiveAsync(ArticleCategoryModifyRequest request);
        Task<bool> DeactivateAsync(ArticleCategoryModifyRequest request);
        Task<bool> DeleteAsync(int id);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
    }
}
