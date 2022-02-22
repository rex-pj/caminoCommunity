using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Products;
using System.Threading.Tasks;
using System.Linq;
using LinqToDB;
using Camino.Shared.Requests.Filters;
using System;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using Camino.Core.Domain.Products;
using Camino.Core.Domain.Media;
using System.Collections.Generic;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Shared.Requests.Products;
using LinqToDB.Tools;
using Camino.Core.Contracts.DependencyInjection;
using Camino.Infrastructure.Linq2Db.Extensions;

namespace Camino.Infrastructure.Repositories.Products
{
    public class ProductPictureRepository : IProductPictureRepository, IScopedDependency
    {
        private readonly IEntityRepository<ProductPicture> _productPictureRepository;
        private readonly IEntityRepository<Picture> _pictureRepository;
        private readonly IEntityRepository<Product> _productRepository;

        public ProductPictureRepository(IEntityRepository<ProductPicture> productPictureRepository,
            IEntityRepository<Picture> pictureRepository, IEntityRepository<Product> productRepository)
        {
            _productPictureRepository = productPictureRepository;
            _pictureRepository = pictureRepository;
            _productRepository = productRepository;
        }

        public async Task<BasePageList<ProductPictureResult>> GetAsync(ProductPictureFilter filter)
        {
            var pictureQuery = _pictureRepository.Get(x => x.StatusId != PictureStatus.Deleted.GetCode());
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                var search = filter.Keyword.ToLower();
                pictureQuery = pictureQuery.Where(pic => pic.Title.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (!string.IsNullOrEmpty(filter.MimeType))
            {
                var mimeType = filter.MimeType.ToLower();
                pictureQuery = pictureQuery.Where(x => x.MimeType.Contains(mimeType));
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTimeOffset.UtcNow);
            }

            var query = from ap in _productPictureRepository.Table
                        join p in pictureQuery
                        on ap.PictureId equals p.Id
                        join a in _productRepository.Table
                        on ap.ProductId equals a.Id
                        select new ProductPictureResult
                        {
                            ProductId = a.Id,
                            ProductName = a.Name,
                            PictureId = p.Id,
                            PictureName = p.FileName,
                            ProductPictureTypeId = ap.PictureTypeId,
                            PictureCreatedById = p.CreatedById,
                            PictureCreatedDate = p.CreatedDate,
                            ContentType = p.MimeType
                        };

            var filteredNumber = query.Select(x => x.PictureId).Count();

            var productPictures = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ProductPictureResult>(productPictures)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public async Task<IList<ProductPictureResult>> GetProductPicturesByProductIdAsync(IdRequestFilter<long> filter, int? productPictureTypeId = null)
        {
            var deletedStatus = PictureStatus.Deleted.GetCode();
            var inactivedStatus = PictureStatus.Inactived.GetCode();
            var productPictures = await (from productPic in _productPictureRepository.Get(x => x.ProductId == filter.Id && (!productPictureTypeId.HasValue || x.PictureTypeId == productPictureTypeId))
                                         join picture in _pictureRepository
                                            .Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
                                          on productPic.PictureId equals picture.Id
                                         select new ProductPictureResult
                                         {
                                             ProductId = productPic.ProductId,
                                             ProductPictureTypeId = productPic.PictureTypeId,
                                             PictureId = productPic.PictureId
                                         }).ToListAsync();
            return productPictures;
        }

        public async Task<IList<ProductPictureResult>> GetProductPicturesByProductIdsAsync(IEnumerable<long> productIds, IdRequestFilter<long> filter, ProductPictureType productPictureType)
        {
            var deletedStatus = PictureStatus.Deleted.GetCode();
            var inactivedStatus = PictureStatus.Inactived.GetCode();
            var productPictureTypeId = productPictureType.GetCode();
            var productPictures = await (from productPic in _productPictureRepository.Get(x => productIds.Contains(x.ProductId) && x.PictureTypeId == productPictureTypeId)
                                         join picture in _pictureRepository
                                         .Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
                                         on productPic.PictureId equals picture.Id
                                         select new ProductPictureResult
                                         {
                                             ProductId = productPic.ProductId,
                                             ProductPictureTypeId = productPic.PictureTypeId,
                                             PictureId = productPic.PictureId
                                         }).ToListAsync();
            return productPictures;
        }

        public async Task<bool> CreateAsync(ProductPicturesModifyRequest request)
        {
            var index = 0;
            foreach (var picture in request.Pictures)
            {
                var binaryPicture = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                var pictureData = Convert.FromBase64String(binaryPicture);
                var pictureId = await _pictureRepository.AddAsync<long>(new Picture()
                {
                    CreatedById = request.UpdatedById,
                    CreatedDate = request.CreatedDate,
                    FileName = picture.FileName,
                    MimeType = picture.ContentType,
                    UpdatedById = request.UpdatedById,
                    UpdatedDate = request.UpdatedDate,
                    BinaryData = pictureData,
                    StatusId = PictureStatus.Pending.GetCode()
                });

                var productPictureTypeId = index == 0 ? (int)ProductPictureType.Thumbnail : (int)ProductPictureType.Secondary;
                await _productPictureRepository.AddAsync(new ProductPicture()
                {
                    ProductId = request.ProductId,
                    PictureId = pictureId,
                    PictureTypeId = productPictureTypeId
                });
                index += 1;
            }

            return true;
        }

        public async Task<bool> UpdateAsync(ProductPicturesModifyRequest request)
        {
            var pictureIds = request.Pictures.Select(x => x.Id);
            var deleteProductPictures = _productPictureRepository
                        .Get(x => x.ProductId == request.ProductId && !pictureIds.Contains(x.PictureId));

            // Delete old images
            var deletePictureIds = deleteProductPictures.Select(x => x.PictureId).ToList();
            if (deletePictureIds.Any())
            {
                await _productPictureRepository.DeleteAsync(deleteProductPictures);
                await _pictureRepository.DeleteAsync(x => deletePictureIds.Contains(x.Id));
            }

            var pictureTypeId = (int)ProductPictureType.Thumbnail;
            var shouldAddPicture = true;
            var hasPicture = _productPictureRepository.Get(x => x.ProductId == request.ProductId && x.PictureTypeId == pictureTypeId).Any();
            if (hasPicture)
            {
                shouldAddPicture = false;
            }

            // Add new images
            foreach (var picture in request.Pictures)
            {
                if (!string.IsNullOrEmpty(picture.Base64Data))
                {
                    var base64Data = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                    var pictureData = Convert.FromBase64String(base64Data);
                    var pictureId = await _pictureRepository.AddAsync<long>(new Picture()
                    {
                        CreatedById = request.UpdatedById,
                        CreatedDate = request.CreatedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = request.UpdatedById,
                        UpdatedDate = request.UpdatedDate,
                        BinaryData = pictureData,
                        StatusId = PictureStatus.Pending.GetCode()
                    });

                    var productPictureTypeId = shouldAddPicture ? pictureTypeId : (int)ProductPictureType.Secondary;
                    _productPictureRepository.Add(new ProductPicture()
                    {
                        ProductId = request.ProductId,
                        PictureId = pictureId,
                        PictureTypeId = productPictureTypeId
                    });
                    shouldAddPicture = false;
                }
            }

            var firstRestPicture = await _productPictureRepository.FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.PictureTypeId != pictureTypeId);
            if (firstRestPicture != null)
            {
                firstRestPicture.PictureTypeId = pictureTypeId;
                await _productPictureRepository.UpdateAsync(firstRestPicture);
            }

            return true;
        }

        public async Task<bool> DeleteByProductIdAsync(long id)
        {
            var productPictures = _productPictureRepository.Get(x => x.ProductId == id);
            var pictureIds = productPictures.Select(x => x.PictureId).ToList();
            await _productPictureRepository.DeleteAsync(productPictures);

            await _pictureRepository.DeleteAsync(x => pictureIds.Contains(x.Id));

            return true;
        }

        public async Task<bool> UpdateStatusByProductIdAsync(ProductPicturesModifyRequest request, PictureStatus pictureStatus)
        {
            await (from productPicture in _productPictureRepository.Get(x => x.ProductId == request.ProductId)
                   join picture in _pictureRepository.Table
                   on productPicture.PictureId equals picture.Id
                   select picture)
                    .SetEntry(x => x.StatusId, pictureStatus.GetCode())
                    .SetEntry(x => x.UpdatedById, request.UpdatedById)
                    .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                    .UpdateAsync();

            return true;
        }

        public async Task<bool> DeleteByProductIdsAsync(IEnumerable<long> ids)
        {
            var productPictures = _productPictureRepository.Get(x => ids.Contains(x.ProductId));
            var pictureIds = productPictures.Select(x => x.PictureId).ToList();
            await _productPictureRepository.DeleteAsync(productPictures);

            await _pictureRepository.DeleteAsync(x => pictureIds.Contains(x.Id));

            return true;
        }

        public async Task<bool> UpdateStatusByProductIdsAsync(IEnumerable<long> ids, long updatedById, PictureStatus status)
        {
            await (from productPicture in _productPictureRepository.Get(x => ids.Contains(x.ProductId))
                   join picture in _pictureRepository.Table
                          on productPicture.PictureId equals picture.Id
                   select picture)
                    .SetEntry(x => x.StatusId, status.GetCode())
                    .SetEntry(x => x.UpdatedById, updatedById)
                    .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                    .UpdateAsync();

            return true;
        }
    }
}
