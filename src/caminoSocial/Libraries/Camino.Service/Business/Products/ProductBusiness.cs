using AutoMapper;
using Camino.Service.Projections.Filters;
using Camino.Data.Contracts;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Products.Contracts;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Data.Enums;
using Camino.Core.Utils;
using System.Collections.Generic;
using Camino.Service.Projections.Media;
using Camino.Service.Projections.Product;

namespace Camino.Service.Business.Products
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<FarmProduct> _farmProductRepository;
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<ProductPrice> _productPriceRepository;
        private readonly IRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ProductBusiness(IMapper mapper, IRepository<Product> productRepository,
            IRepository<ProductCategoryRelation> productCategoryRelationRepository, IRepository<User> userRepository,
            IRepository<Picture> pictureRepository, IRepository<ProductPicture> productPictureRepository,
            IRepository<UserPhoto> userPhotoRepository, IRepository<ProductPrice> productPriceRepository,
            IRepository<FarmProduct> farmProductRepository, IRepository<Farm> farmRepository,
            IRepository<ProductCategory> productCategoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _userRepository = userRepository;
            _pictureRepository = pictureRepository;
            _productPictureRepository = productPictureRepository;
            _userPhotoRepository = userPhotoRepository;
            _productPriceRepository = productPriceRepository;
            _farmProductRepository = farmProductRepository;
            _farmRepository = farmRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<ProductProjection> FindAsync(long id)
        {
            var exist = await (from product in _productRepository.Table
                               where product.Id == id
                               select new ProductProjection
                               {
                                   CreatedDate = product.CreatedDate,
                                   CreatedById = product.CreatedById,
                                   Id = product.Id,
                                   Name = product.Name,
                                   UpdatedById = product.UpdatedById,
                                   UpdatedDate = product.UpdatedDate,
                               }).FirstOrDefaultAsync();

            return exist;
        }

        public async Task<ProductProjection> FindDetailAsync(long id)
        {
            var farmQuery = from farm in _farmRepository.Table
                            join farmProduct in _farmProductRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var productCategoryQuery = from category in _productCategoryRepository.Table
                                       join categoryRelation in _productCategoryRelationRepository.Table
                                       on category.Id equals categoryRelation.ProductCategoryId
                                       select new
                                       {
                                           Id = categoryRelation.Id,
                                           CategoryId = category.Id,
                                           ProductId = categoryRelation.ProductId,
                                           Name = category.Name
                                       };

            var product = await (from p in _productRepository.Table
                                 join pr in _productPriceRepository.Get(x => x.IsCurrent)
                                 on p.Id equals pr.ProductId into prices
                                 from price in prices.DefaultIfEmpty()

                                 join productPic in _productPictureRepository.Table
                                 on p.Id equals productPic.ProductId into pics

                                 join categoryRelation in productCategoryQuery
                                 on p.Id equals categoryRelation.ProductId into productCategories

                                 join fp in farmQuery
                                 on p.Id equals fp.ProductId into farmProducts
                                 where p.Id == id
                                 select new ProductProjection
                                 {
                                     Description = p.Description,
                                     CreatedDate = p.CreatedDate,
                                     CreatedById = p.CreatedById,
                                     Id = p.Id,
                                     Name = p.Name,
                                     UpdatedById = p.UpdatedById,
                                     UpdatedDate = p.UpdatedDate,
                                     Price = price.Price,
                                     ProductCategories = productCategories.Select(x => new ProductCategoryProjection()
                                     {
                                         Id = x.CategoryId,
                                         Name = x.Name
                                     }),
                                     ProductFarms = farmProducts.Select(x => new ProductFarmProjection()
                                     {
                                         Id = x.Id,
                                         FarmId = x.FarmId,
                                         FarmName = x.Name
                                     }),
                                     Pictures = pics.Select(x => new PictureRequestProjection
                                     {
                                         Id = x.PictureId
                                     }),
                                 }).FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
            }

            var createdByUserName = await _userRepository.Get(x => x.Id == product.CreatedById).Select(x => x.DisplayName).FirstOrDefaultAsync();
            product.CreatedBy = createdByUserName;

            return product;
        }

        public ProductProjection FindByName(string name)
        {
            var exist = _productRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var product = _mapper.Map<ProductProjection>(exist);

            return product;
        }

        public async Task<BasePageList<ProductProjection>> GetAsync(ProductFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var productQuery = _productRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                productQuery = productQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                productQuery = productQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.CategoryId.HasValue)
            {
                productQuery = productQuery.Where(x => x.ProductCategories.Any(c => c.ProductCategoryId == filter.CategoryId));
            }

            if (filter.FarmId.HasValue)
            {
                productQuery = productQuery.Where(x => x.ProductFarms.Any(c => c.FarmId == filter.FarmId));
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var filteredNumber = productQuery.Select(x => x.Id).Count();

            var farmQuery = from farm in _farmRepository.Table
                            join farmProduct in _farmProductRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var thumbnailTypeId = (byte)ProductPictureType.Thumbnail;
            var query = from product in productQuery
                        join productPic in _productPictureRepository.Get(x => x.PictureType == thumbnailTypeId)
                        on product.Id equals productPic.ProductId into pics
                        join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                        on product.CreatedById equals pho.CreatedById into photos
                        from userPhoto in photos.DefaultIfEmpty()
                        join pr in _productPriceRepository.Get(x => x.IsCurrent)
                        on product.Id equals pr.ProductId into prices
                        from price in prices.DefaultIfEmpty()
                        join fp in farmQuery
                        on product.Id equals fp.ProductId into farmProducts
                        select new ProductProjection
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = price != null ? price.Price : 0,
                            CreatedById = product.CreatedById,
                            CreatedDate = product.CreatedDate,
                            Description = product.Description,
                            UpdatedById = product.UpdatedById,
                            UpdatedDate = product.UpdatedDate,
                            CreatedByPhotoCode = userPhoto.Code,
                            Pictures = pics.Select(x => new PictureRequestProjection
                            {
                                Id = x.PictureId
                            }),
                            ProductFarms = farmProducts.Select(x => new ProductFarmProjection
                            {
                                Id = x.Id,
                                FarmId = x.FarmId,
                                FarmName = x.Name
                            })
                        };

            var products = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var createdByIds = products.Select(x => x.CreatedById).ToArray();
            var updatedByIds = products.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();

            foreach (var product in products)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == product.CreatedById);
                product.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == product.UpdatedById);
                product.UpdatedBy = updatedBy.DisplayName;
            }

            var result = new BasePageList<ProductProjection>(products)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<IList<ProductProjection>> GetRelevantsAsync(long id, ProductFilter filter)
        {
            var exist = (from pr in _productRepository.Get(x => x.Id == id)
                         join fp in _farmProductRepository.Table
                         on pr.Id equals fp.ProductId into farmProducts
                         join productCategoryRelation in _productCategoryRelationRepository.Table
                         on pr.Id equals productCategoryRelation.ProductId into categoriesRelation
                         select new ProductProjection
                         {
                             Id = pr.Id,
                             CreatedById = pr.CreatedById,
                             UpdatedById = pr.UpdatedById,
                             ProductCategories = categoriesRelation.Select(x => new ProductCategoryProjection()
                             {
                                 Id = x.ProductCategoryId
                             }),
                             ProductFarms = farmProducts.Select(x => new ProductFarmProjection()
                             {
                                 FarmId = x.FarmId
                             })
                         }).FirstOrDefault();

            var farmIds = exist.ProductFarms.Select(x => x.FarmId);
            var categoryIds = exist.ProductCategories.Select(x => x.Id);

            var farmQuery = from farm in _farmRepository.Get(x => farmIds.Contains(x.Id))
                            join farmProduct in _farmProductRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var thumbnailTypeId = (byte)ProductPictureType.Thumbnail;
            var relevantProductQuery = (from pr in _productRepository.Get(x => x.Id != exist.Id)
                                        join fp in farmQuery
                                        on pr.Id equals fp.ProductId into farmProducts

                                        join productCategoryRelation in _productCategoryRelationRepository.Table
                                        on pr.Id equals productCategoryRelation.ProductId into categoriesRelation
                                        from categoryRelation in categoriesRelation.DefaultIfEmpty()

                                        join productPic in _productPictureRepository.Get(x => x.PictureType == thumbnailTypeId)
                                        on pr.Id equals productPic.ProductId into pics
                                        join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                                        on pr.CreatedById equals pho.CreatedById into photos
                                        from userPhoto in photos.DefaultIfEmpty()

                                        join prc in _productPriceRepository.Get(x => x.IsCurrent)
                                        on pr.Id equals prc.ProductId into prices
                                        from price in prices.DefaultIfEmpty()

                                        where pr.CreatedById == exist.CreatedById
                                        || categoryIds.Contains(categoryRelation.ProductCategoryId)
                                        || pr.UpdatedById == exist.UpdatedById
                                        select new ProductProjection
                                        {
                                            Id = pr.Id,
                                            Name = pr.Name,
                                            Price = price != null ? price.Price : 0,
                                            CreatedById = pr.CreatedById,
                                            CreatedDate = pr.CreatedDate,
                                            Description = pr.Description,
                                            UpdatedById = pr.UpdatedById,
                                            UpdatedDate = pr.UpdatedDate,
                                            CreatedByPhotoCode = userPhoto.Code,
                                            Pictures = pics.Select(x => new PictureRequestProjection
                                            {
                                                Id = x.PictureId
                                            }),
                                            ProductFarms = farmProducts.Select(x => new ProductFarmProjection
                                            {
                                                Id = x.Id,
                                                FarmId = x.FarmId,
                                                FarmName = x.Name
                                            })
                                        });

            var relevantProducts = await relevantProductQuery
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var createdByIds = relevantProducts.Select(x => x.CreatedById).ToArray();
            var updatedByIds = relevantProducts.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();

            foreach (var product in relevantProducts)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == product.CreatedById);
                product.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == product.UpdatedById);
                product.UpdatedBy = updatedBy.DisplayName;
            }

            return relevantProducts;
        }

        public async Task<long> CreateAsync(ProductProjection product)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var newProduct = new Product()
            {
                CreatedById = product.CreatedById,
                UpdatedById = product.UpdatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Description = product.Description,
                Name = product.Name,
            };

            var id = await _productRepository.AddWithInt64EntityAsync(newProduct);
            if (id > 0)
            {
                foreach (var category in product.ProductCategories)
                {
                    _productCategoryRelationRepository.Add(new ProductCategoryRelation()
                    {
                        ProductCategoryId = category.Id,
                        ProductId = id
                    });
                }

                foreach (var farm in product.ProductFarms)
                {
                    _farmProductRepository.Add(new FarmProduct()
                    {
                        FarmId = farm.FarmId,
                        ProductId = id,
                        IsLinked = true,
                        LinkedById = product.CreatedById,
                        LinkedDate = modifiedDate
                    });
                }

                _productPriceRepository.Add(new ProductPrice()
                {
                    PricedDate = modifiedDate,
                    ProductId = id,
                    Price = product.Price,
                    IsCurrent = true
                });

                var index = 0;
                foreach (var picture in product.Pictures)
                {
                    var thumbnail = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                    var pictureData = Convert.FromBase64String(thumbnail);
                    var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                    {
                        CreatedById = product.UpdatedById,
                        CreatedDate = modifiedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = product.UpdatedById,
                        UpdatedDate = modifiedDate,
                        BinaryData = pictureData
                    });

                    var productPictureType = index == 0 ? (int)ProductPictureType.Thumbnail : (int)ProductPictureType.Secondary;
                    _productPictureRepository.Add(new ProductPicture()
                    {
                        ProductId = id,
                        PictureId = pictureId,
                        PictureType = productPictureType
                    });
                    index += 1;
                }
            }

            return id;
        }

        public async Task<ProductProjection> UpdateAsync(ProductProjection request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var product = _productRepository.FirstOrDefault(x => x.Id == request.Id);
            product.Description = request.Description;
            product.Name = request.Name;
            product.UpdatedById = request.UpdatedById;
            product.UpdatedDate = modifiedDate;

            var pictureIds = request.Pictures.Select(x => x.Id);
            var deleteProductPictures = _productPictureRepository
                        .Get(x => x.ProductId == request.Id && !pictureIds.Contains(x.PictureId));

            // Delete old images
            var deletePictureIds = deleteProductPictures.Select(x => x.PictureId).ToList();
            if (deletePictureIds.Any())
            {
                await _productPictureRepository.DeleteAsync(deleteProductPictures);

                var currentPictures = _pictureRepository.Get(x => deletePictureIds.Contains(x.Id));
                await _pictureRepository.DeleteAsync(currentPictures);
            }

            var thumbnailType = (int)ProductPictureType.Thumbnail;
            var shouldAddThumbnail = true;
            var hasThumbnail = _productPictureRepository.Get(x => x.ProductId == request.Id && x.PictureType == thumbnailType).Any();
            if (hasThumbnail)
            {
                shouldAddThumbnail = false;
            }

            // Add new images
            foreach (var picture in request.Pictures)
            {
                if (!string.IsNullOrEmpty(picture.Base64Data))
                {
                    var base64Data = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                    var pictureData = Convert.FromBase64String(base64Data);
                    var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                    {
                        CreatedById = request.UpdatedById,
                        CreatedDate = modifiedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = request.UpdatedById,
                        UpdatedDate = modifiedDate,
                        BinaryData = pictureData
                    });

                    var farmPictureType = shouldAddThumbnail ? thumbnailType : (int)ProductPictureType.Secondary;
                    _productPictureRepository.Add(new ProductPicture()
                    {
                        ProductId = product.Id,
                        PictureId = pictureId,
                        PictureType = farmPictureType
                    });
                    shouldAddThumbnail = false;
                }
            }

            var firstRestPicture = await _productPictureRepository.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.PictureType != thumbnailType);
            if (firstRestPicture != null)
            {
                firstRestPicture.PictureType = thumbnailType;
                await _productPictureRepository.UpdateAsync(firstRestPicture);
            }

            // Update Category
            var categoryIds = request.ProductCategories.Select(x => x.Id);
            var deleteCategories = _productCategoryRelationRepository
                        .Get(x => x.ProductId == request.Id && !categoryIds.Contains(x.ProductCategoryId));

            var deletecategoryIds = deleteCategories.Select(x => x.ProductCategoryId).ToList();
            if (deletecategoryIds.Any())
            {
                await _productCategoryRelationRepository.DeleteAsync(deleteCategories);
            }

            var linkedCategoryIds = _productCategoryRelationRepository
                .Get(x => x.ProductId == request.Id && categoryIds.Contains(x.ProductCategoryId))
                .Select(x => x.ProductCategoryId)
                .ToList();

            var unlinkedCategories = request.ProductCategories.Where(x => !linkedCategoryIds.Contains(x.Id));
            if (unlinkedCategories != null && unlinkedCategories.Any())
            {
                foreach (var category in unlinkedCategories)
                {
                    _productCategoryRelationRepository.Add(new ProductCategoryRelation()
                    {
                        ProductCategoryId = category.Id,
                        ProductId = request.Id
                    });
                }
            }

            // Update Farm
            var farmIds = request.ProductFarms.Select(x => x.FarmId);
            var deletedFarms = _farmProductRepository
                        .Get(x => x.ProductId == request.Id && !farmIds.Contains(x.FarmId));

            var deleteFarmIds = deletedFarms.Select(x => x.FarmId).ToList();
            if (deleteFarmIds.Any())
            {
                await _farmProductRepository.DeleteAsync(deletedFarms);
            }

            var linkedFarmIds = _farmProductRepository
                .Get(x => x.ProductId == request.Id && farmIds.Contains(x.FarmId))
                .Select(x => x.FarmId)
                .ToList();

            var unlinkedFarms = request.ProductFarms.Where(x => !linkedFarmIds.Contains(x.FarmId));
            if (unlinkedFarms != null && unlinkedFarms.Any())
            {
                foreach (var farm in unlinkedFarms)
                {
                    _farmProductRepository.Add(new FarmProduct()
                    {
                        FarmId = farm.FarmId,
                        ProductId = request.Id,
                        IsLinked = true,
                        LinkedById = product.CreatedById,
                        LinkedDate = modifiedDate
                    });
                }
            }

            // Unlink all price
            var unlinkedProductPrices = await _productPriceRepository.Get(x => x.ProductId == request.Id && x.IsCurrent && x.Price != request.Price)
                .ToListAsync();

            if (unlinkedProductPrices.Any())
            {
                unlinkedProductPrices.ForEach(x => x.IsCurrent = false);
                await _productPriceRepository.UpdateAsync(unlinkedProductPrices);

                await _productPriceRepository.AddAsync(new ProductPrice()
                {
                    PricedDate = modifiedDate,
                    ProductId = request.Id,
                    Price = request.Price,
                    IsCurrent = true
                });
            }

            _productRepository.Update(product);

            return request;
        }
    }
}
