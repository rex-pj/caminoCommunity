using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Articles.Repositories;
using Camino.Core.Domains.Media;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Products.DomainServices;
using Camino.Shared.Enums;
using Camino.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Core.DomainServices.Products
{
    public class ProductPictureDomainService : IProductPictureDomainService, IScopedDependency
    {
        private readonly IEntityRepository<Picture> _pictureRepository;
        private readonly IEntityRepository<ProductPicture> _productPictureRepository;
        private readonly IDbContext _dbContext;

        public ProductPictureDomainService(IEntityRepository<ProductPicture> productPictureRepository,
            IEntityRepository<Picture> pictureRepository,
            IDbContext dbContext)
        {
            _pictureRepository = pictureRepository;
            _dbContext = dbContext;
            _productPictureRepository = productPictureRepository;
        }

        public async Task DeleteByProductIdAsync(long productId, bool needSaveChanges = false)
        {
            var productPictures = _productPictureRepository.Get(x => x.ProductId == productId);
            var pictureIds = productPictures.Select(x => x.PictureId).ToList();

            await DeleteByProductIdAsync(productId, pictureIds, needSaveChanges);
        }

        public async Task DeleteByProductIdAsync(long productId, IList<long> pictureIds, bool needSaveChanges = false)
        {
            var deleteProductPictures = _productPictureRepository
                        .Get(x => x.ProductId == productId && !pictureIds.Contains(x.PictureId));

            // Delete old images
            var deletePictureIds = deleteProductPictures.Select(x => x.PictureId).ToList();
            if (deletePictureIds.Any())
            {
                await _productPictureRepository.DeleteAsync(deleteProductPictures);
                await _pictureRepository.DeleteAsync(x => deletePictureIds.Contains(x.Id));
            }

            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteByProductIdsAsync(IEnumerable<long> ids, bool needSaveChanges = false)
        {
            var productPictures = _productPictureRepository.Get(x => ids.Contains(x.ProductId));
            var pictureIds = productPictures.Select(x => x.PictureId).ToList();
            await _productPictureRepository.DeleteAsync(productPictures);

            await _pictureRepository.DeleteAsync(x => pictureIds.Contains(x.Id));
            if (needSaveChanges)
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }

            return false;
        }

        public async Task<bool> UpdateStatusByProductIdAsync(long productId, long updatedById, PictureStatuses pictureStatus)
        {
            var existingPictures = (from productPicture in _productPictureRepository.Get(x => x.ProductId == productId)
                                    join picture in _pictureRepository.Table
                                    on productPicture.PictureId equals picture.Id
                                    select picture);

            foreach (var picture in existingPictures)
            {
                picture.StatusId = pictureStatus.GetCode();
                picture.UpdatedById = updatedById;
                picture.UpdatedDate = DateTime.UtcNow;
            }

            await _pictureRepository.UpdateAsync(existingPictures);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateStatusByProductIdsAsync(IEnumerable<long> productIds, long updatedById, PictureStatuses status)
        {
            var existingPictures = (from productPicture in _productPictureRepository.Get(x => productIds.Contains(x.ProductId))
                                    join picture in _pictureRepository.Table
                                           on productPicture.PictureId equals picture.Id
                                    select picture);

            foreach (var picture in existingPictures)
            {
                picture.StatusId = status.GetCode();
                picture.UpdatedById = updatedById;
                picture.UpdatedDate = DateTime.UtcNow;
            }

            await _pictureRepository.UpdateAsync(existingPictures);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
