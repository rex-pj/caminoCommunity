using Camino.Shared.Results.Articles;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Requests.Articles;
using Camino.Shared.Enums;

namespace Camino.Core.Contracts.Repositories.Articles
{
    public interface IArticlePictureRepository
    {
        Task<BasePageList<ArticlePictureResult>> GetAsync(ArticlePictureFilter filter);
        Task<ArticlePictureResult> GetArticlePictureByArticleIdAsync(long articleId);
        Task<IList<ArticlePictureResult>> GetArticlePicturesByArticleIdsAsync(IEnumerable<long> articleIds);
        Task<long> CreateAsync(ArticlePictureModifyRequest request);
        Task<bool> UpdateAsync(ArticlePictureModifyRequest request);
        Task<bool> DeleteByArticleIdAsync(long articleId);
        Task<bool> UpdateStatusByArticleIdAsync(ArticlePictureModifyRequest request, PictureStatus pictureStatus);
    }
}
