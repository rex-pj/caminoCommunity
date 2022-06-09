using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Application.Contracts.AppServices.Articles.Dtos.Dtos;
using Camino.Shared.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Camino.Application.Contracts.AppServices.Articles
{
    public interface IArticlePictureAppService
    {
        Task<long> CreateAsync(ArticlePictureModifyRequest request);
        Task<BasePageList<ArticlePictureResult>> GetAsync(ArticlePictureFilter filter);
        Task<ArticlePictureResult> GetByArticleIdAsync(IdRequestFilter<long> filter);
        Task<IList<ArticlePictureResult>> GetListByArticleIdsAsync(IEnumerable<long> articleIds, IdRequestFilter<long> filter);
        Task<IList<ArticlePictureResult>> GetListByArticleIdsAsync(IEnumerable<long> articleIds, IdRequestFilter<long> filter, ArticlePictureTypes? articlePictureType);
        Task<bool> UpdateAsync(ArticlePictureModifyRequest request);
    }
}