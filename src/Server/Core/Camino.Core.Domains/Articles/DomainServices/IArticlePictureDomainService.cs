using Camino.Shared.Enums;

namespace Camino.Core.Domains.Articles.DomainServices
{
    public interface IArticlePictureDomainService
    {
        Task<bool> DeleteByArticleIdAsync(long articleId, bool needSaveChanges = false);
        Task<bool> UpdateStatusByArticleIdAsync(long articleId, long updatedById, PictureStatuses pictureStatus);
    }
}