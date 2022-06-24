using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Application.Contracts.AppServices.Products.Dtos;
using Camino.Core.Contracts.Repositories.Media;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Media;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Products.DomainServices;
using Camino.Shared.Enums;
using Camino.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Camino.Application.AppServices.Products
{
    public class ProductPictureAppService : IProductPictureAppService, IScopedDependency
    {
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IEntityRepository<Picture> _pictureEntityRepository;
        private readonly IEntityRepository<ProductPicture> _productPictureEntityRepository;
        private readonly IEntityRepository<Product> _productRepository;
        private readonly IProductPictureDomainService _productPictureDomainService;
        private readonly IPictureRepository _pictureRepository;
        private readonly IDbContext _dbContext;
        private readonly int _deletedStatus = PictureStatuses.Deleted.GetCode();
        private readonly int _inactivedStatus = PictureStatuses.Inactived.GetCode();

        public ProductPictureAppService(IDbContext dbContext,
            IPictureRepository pictureRepository,
            IEntityRepository<ProductPicture> productPictureEntityRepository,
            IProductPictureRepository productPictureRepository,
            IEntityRepository<Picture> pictureEntityRepository,
            IEntityRepository<Product> productRepository,
            IProductPictureDomainService productPictureDomainService)
        {
            _productPictureEntityRepository = productPictureEntityRepository;
            _pictureEntityRepository = pictureEntityRepository;
            _productRepository = productRepository;
            _pictureRepository = pictureRepository;
            _productPictureRepository = productPictureRepository;
            _productPictureDomainService = productPictureDomainService;
            _dbContext = dbContext;
        }

        public async Task<BasePageList<ProductPictureResult>> GetAsync(ProductPictureFilter filter)
        {
            var pictureQuery = _pictureEntityRepository.Get(x => x.StatusId != PictureStatuses.Deleted.GetCode());
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

            var query = from ap in _productPictureEntityRepository.Table
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

        public async Task<IList<ProductPictureResult>> GetListByProductIdAsync(IdRequestFilter<long> filter, int? productPictureTypeId = null)
        {
            var productPictures = await (from productPic in _productPictureEntityRepository.Get(x => x.ProductId == filter.Id && (!productPictureTypeId.HasValue || x.PictureTypeId == productPictureTypeId))
                                         join picture in _pictureEntityRepository
                                            .Get(x => (x.StatusId == _deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == _inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != _deletedStatus && x.StatusId != _inactivedStatus))
                                          on productPic.PictureId equals picture.Id
                                         select new ProductPictureResult
                                         {
                                             ProductId = productPic.ProductId,
                                             ProductPictureTypeId = productPic.PictureTypeId,
                                             PictureId = productPic.PictureId
                                         }).ToListAsync();
            return productPictures;
        }

        public async Task<IList<ProductPictureResult>> GetListByProductIdsAsync(IEnumerable<long> productIds, IdRequestFilter<long> filter, ProductPictureTypes productPictureType)
        {
            var productPictureTypeId = productPictureType.GetCode();
            var productPictures = await (from productPic in _productPictureEntityRepository.Get(x => productIds.Contains(x.ProductId) && x.PictureTypeId == productPictureTypeId)
                                         join picture in _pictureEntityRepository
                                         .Get(x => (x.StatusId == _deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == _inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != _deletedStatus && x.StatusId != _inactivedStatus))
                                         on productPic.PictureId equals picture.Id
                                         select new ProductPictureResult
                                         {
                                             ProductId = productPic.ProductId,
                                             ProductPictureTypeId = productPic.PictureTypeId,
                                             PictureId = productPic.PictureId
                                         }).ToListAsync();
            return productPictures;
        }

        public async Task<bool> CreateAsync(ProductPicturesModifyRequest request, bool needSaveChanges = false)
        {
            var index = 0;
            foreach (var picture in request.Pictures)
            {
                var pictureType = index == 0 ? ProductPictureTypes.Thumbnail : ProductPictureTypes.Secondary;
                await CreateAsync(request, picture, pictureType);
                index += 1;
            }

            if (needSaveChanges)
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }

            return false;
        }

        public async Task<bool> UpdateAsync(ProductPicturesModifyRequest request, bool needSaveChanges = false)
        {
            // Delete old images
            var pictureIds = request.Pictures.Select(x => x.Id).ToList();
            await _productPictureDomainService.DeleteByProductIdAsync(request.ProductId, pictureIds);

            var thumbnailType = ProductPictureTypes.Thumbnail;
            var thumbnail = await _productPictureRepository.GetByTypeAsync(request.ProductId, thumbnailType);
            var shouldAddThumbnail = thumbnail == null;

            // Add new images
            var index = 0;
            foreach (var picture in request.Pictures)
            {
                if (!string.IsNullOrEmpty(picture.Base64Data))
                {
                    var productPictureType = shouldAddThumbnail && index == 0 ? thumbnailType : ProductPictureTypes.Secondary;
                    await CreateAsync(request, picture, productPictureType, needSaveChanges);
                    shouldAddThumbnail = false;
                    index += 1;
                }
            }

            if (needSaveChanges)
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }

            return false;
        }

        private async Task<long> CreateAsync(ProductPicturesModifyRequest request, PictureRequest picture, ProductPictureTypes pictureType, bool needSaveChanges = false)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var binaryPicture = ImageUtils.EncodeJavascriptBase64(picture.Base64Data);
            var pictureData = Convert.FromBase64String(binaryPicture);
            var pictureId = await _pictureRepository.CreateAsync(new Picture
            {
                CreatedById = request.UpdatedById,
                CreatedDate = modifiedDate,
                FileName = picture.FileName,
                MimeType = picture.ContentType,
                UpdatedById = request.UpdatedById,
                UpdatedDate = modifiedDate,
                BinaryData = pictureData,
                StatusId = PictureStatuses.Pending.GetCode()
            });

            return await _productPictureRepository.CreateAsync(new ProductPicture
            {
                ProductId = request.ProductId,
                PictureId = pictureId,
                PictureTypeId = pictureType.GetCode()
            }, needSaveChanges);
        }
    }
}
