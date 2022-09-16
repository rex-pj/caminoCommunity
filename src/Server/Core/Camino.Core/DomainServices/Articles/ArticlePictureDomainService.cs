using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Articles;
using Camino.Core.Domains.Articles.DomainServices;
using Camino.Core.Domains.Media;
using Camino.Shared.Enums;
using Camino.Shared.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Core.DomainServices.Articles
{
    public class ArticlePictureDomainService : IArticlePictureDomainService, IScopedDependency
    {
        private readonly IEntityRepository<ArticlePicture> _articlePictureEntityRepository;
        private readonly IEntityRepository<Picture> _pictureEntityRepository;
        private readonly IDbContext _dbContext;
        public ArticlePictureDomainService(IEntityRepository<ArticlePicture> articlePictureEntityRepository,
           IEntityRepository<Picture> pictureEntityRepository,
           IDbContext dbContext)
        {
            _articlePictureEntityRepository = articlePictureEntityRepository;
            _pictureEntityRepository = pictureEntityRepository;
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteByArticleIdAsync(long articleId, bool needSaveChanges = false)
        {
            var articlePictures = _articlePictureEntityRepository.Get(x => x.ArticleId == articleId);
            if (articlePictures == null || !articlePictures.Any())
            {
                return false;
            }

            var pictureIds = articlePictures.Select(x => x.PictureId).ToList();
            await _articlePictureEntityRepository.DeleteAsync(articlePictures);

            await _pictureEntityRepository.DeleteAsync(x => pictureIds.Contains(x.Id));
            if (needSaveChanges)
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }
            return false;
        }

        public async Task<bool> UpdateStatusByArticleIdAsync(long articleId, long updatedById, PictureStatuses pictureStatus)
        {
            var existingPictures = (from articlePicture in _articlePictureEntityRepository.Get(x => x.ArticleId == articleId)
                                    join picture in _pictureEntityRepository.Table
                                    on articlePicture.PictureId equals picture.Id
                                    select picture).ToList();

            foreach (var picture in existingPictures)
            {
                picture.StatusId = pictureStatus.GetCode();
                picture.UpdatedById = updatedById;
                picture.UpdatedDate = DateTime.UtcNow;
            }

            await _pictureEntityRepository.UpdateAsync(existingPictures);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
