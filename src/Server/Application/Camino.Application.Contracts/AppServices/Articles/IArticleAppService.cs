using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Application.Contracts.AppServices.Articles.Dtos.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Camino.Application.Contracts.AppServices.Articles
{
    public interface IArticleAppService
    {
        Task<bool> ActiveAsync(ArticleModifyRequest request);
        Task<long> CreateAsync(ArticleModifyRequest request);
        Task<bool> DeactivateAsync(ArticleModifyRequest request);
        Task<bool> DeleteAsync(long id);
        Task<ArticleResult> FindAsync(IdRequestFilter<long> filter);
        Task<ArticleResult> FindByNameAsync(string name);
        Task<ArticleResult> FindDetailAsync(IdRequestFilter<long> filter);
        Task<BasePageList<ArticleResult>> GetAsync(ArticleFilter filter);
        Task<IList<ArticleResult>> GetRelevantsAsync(long id, ArticleFilter filter);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
        Task<bool> SoftDeleteAsync(ArticleModifyRequest request);
        Task<bool> UpdateAsync(ArticleModifyRequest request);
    }
}