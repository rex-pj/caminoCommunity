using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using System.Collections.Generic;
using Camino.Shared.Requests.Articles;
using Camino.Shared.General;

namespace Camino.Core.Contracts.Services.Articles
{
    public interface IArticleService
    {
        Task<int> CreateAsync(ArticleModifyRequest article);
        Task<ArticleResult> FindAsync(IdRequestFilter<long> filter);
        Task<ArticleResult> FindDetailAsync(IdRequestFilter<long> filter);
        ArticleResult FindByName(string name);
        Task<bool> UpdateAsync(ArticleModifyRequest article);
        Task<BasePageList<ArticleResult>> GetAsync(ArticleFilter filter);
        Task<IList<ArticleResult>> GetRelevantsAsync(long id, ArticleFilter filter);
        Task<bool> DeleteAsync(long id);
        Task<bool> SoftDeleteAsync(ArticleModifyRequest request);
        Task<bool> DeactivateAsync(ArticleModifyRequest request);
        Task<bool> ActiveAsync(ArticleModifyRequest request);
        Task<BasePageList<ArticlePictureResult>> GetPicturesAsync(ArticlePictureFilter filter);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
    }
}