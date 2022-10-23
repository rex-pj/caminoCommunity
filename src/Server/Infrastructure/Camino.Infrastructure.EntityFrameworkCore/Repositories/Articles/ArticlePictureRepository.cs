using System.Threading.Tasks;
using Camino.Core.Domains.Articles.Repositories;
using Camino.Core.Domains.Articles;
using Camino.Shared.Enums;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Shared.Utils;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Articles
{
    public class ArticlePictureRepository : IArticlePictureRepository, IScopedDependency
    {
        private readonly IEntityRepository<ArticlePicture> _articlePictureRepository;
        private readonly IDbContext _dbContext;

        public ArticlePictureRepository(IEntityRepository<ArticlePicture> articlePictureRepository, 
            IDbContext dbContext)
        {
            _articlePictureRepository = articlePictureRepository;
            _dbContext = dbContext;
        }

        public async Task<ArticlePicture> GetByTypeAsync(long articleId, ArticlePictureTypes pictureType)
        {
            return await _articlePictureRepository
                .FindAsync(x => x.ArticleId == articleId && x.PictureTypeId == pictureType.GetCode());
        }

        public async Task<long> CreateAsync(ArticlePicture articlePicture, bool needSaveChanges = false)
        {
            await _articlePictureRepository.InsertAsync(articlePicture);
            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
                return articlePicture.Id;
            }
            return -1;
        }

        public async Task UpdateAsync(ArticlePicture articlePicture, bool needSaveChanges = false)
        {
            await _articlePictureRepository.UpdateAsync(articlePicture);
            if (needSaveChanges)
            {
                return;
            }
        }
    }
}
