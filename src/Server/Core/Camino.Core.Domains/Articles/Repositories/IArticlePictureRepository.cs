using Camino.Shared.Enums;

namespace Camino.Core.Domains.Articles.Repositories
{
    public interface IArticlePictureRepository
    {
        Task<ArticlePicture> GetByTypeAsync(long articleId, ArticlePictureTypes pictureType);
        Task<long> CreateAsync(ArticlePicture articlePicture, bool needSaveChanges = false);
        Task UpdateAsync(ArticlePicture articlePicture, bool needSaveChanges = false);
    }
}
