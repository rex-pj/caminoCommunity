using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using System.Collections.Generic;
using Camino.Shared.Requests.Articles;

namespace Camino.Core.Contracts.Services.Articles
{
    public interface IArticleService
    {
        Task<int> CreateAsync(ArticleModifyRequest article);
        Task<ArticleResult> FindAsync(long id);
        Task<ArticleResult> FindDetailAsync(long id);
        ArticleResult FindByName(string name);
        Task<bool> UpdateAsync(ArticleModifyRequest article);
        Task<BasePageList<ArticleResult>> GetAsync(ArticleFilter filter);
        Task<IList<ArticleResult>> GetRelevantsAsync(long id, ArticleFilter filter);
        Task<bool> DeleteAsync(long id);
        Task<bool> SoftDeleteAsync(long id);
        Task<bool> DeactivateAsync(long id);
        Task<BasePageList<ArticlePictureResult>> GetPicturesAsync(ArticlePictureFilter filter);
    }
}