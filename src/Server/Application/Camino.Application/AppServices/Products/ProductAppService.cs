using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Application.Contracts.AppServices.Products.Dtos;
using Camino.Application.Contracts.Utils;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Farms;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Products.DomainServices;
using Camino.Infrastructure.EntityFrameworkCore;
using Camino.Shared.Enums;
using Camino.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Camino.Application.AppServices.Products
{
    public class ProductAppService : IProductAppService, IScopedDependency
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly IProductAttributeDomainService _productAttributeDomainService;
        private readonly IProductAttributeAppService _productAttributeAppService;
        private readonly IProductPictureAppService _productPictureAppService;
        private readonly IProductPictureDomainService _productPictureDomainService;
        private readonly IProductCategoryDomainService _productCategoryDomainService;
        private readonly IProductFarmDomainService _productFarmDomainService;
        private readonly IProductPriceDomainService _productPriceDomainService;
        private readonly IEntityRepository<Product> _productEntityRepository;
        private readonly IEntityRepository<Farm> _farmEntityRepository;
        private readonly IEntityRepository<FarmProduct> _farmProductEntityRepository;
        private readonly IEntityRepository<ProductCategory> _productCategoryEntityRepository;
        private readonly IEntityRepository<ProductPrice> _productPriceEntityRepository;
        private readonly IEntityRepository<ProductCategoryRelation> _productCategoryRelationEntityRepository;
        private readonly IAppDbContext _dbContext;
        private readonly int _deletedStatus = ProductStatuses.Deleted.GetCode();
        private readonly int _inactivedStatus = ProductStatuses.Inactived.GetCode();
        private readonly int _activedStatus = ProductStatuses.Actived.GetCode();
        private readonly int _pendingStatus = ProductStatuses.Pending.GetCode();
        private readonly int _farmDeletedStatus = FarmStatuses.Deleted.GetCode();
        private readonly int _farmInactivedStatus = FarmStatuses.Inactived.GetCode();

        public ProductAppService(IAppDbContext dbContext, 
            IProductRepository productRepository,
            IUserRepository userRepository, IUserPhotoRepository userPhotoRepository,
            IProductAttributeDomainService productAttributeDomainService,
            IProductAttributeAppService productAttributeAppService,
            IProductPictureDomainService productPictureDomainService,
            IProductCategoryDomainService productCategoryDomainService,
            IProductFarmDomainService productFarmDomainService,
            IProductPriceDomainService productPriceDomainService,
            IProductPictureAppService productPictureAppService,
            IEntityRepository<Farm> farmEntityRepository,
            IEntityRepository<FarmProduct> farmProductEntityRepository,
            IEntityRepository<ProductCategory> productCategoryEntityRepository,
            IEntityRepository<ProductCategoryRelation> productCategoryRelationEntityRepository,
            IEntityRepository<ProductPrice> productPriceEntityRepository,
            IEntityRepository<Product> productEntityRepository)
        {
            _productRepository = productRepository;
            _productPictureAppService = productPictureAppService;
            _userRepository = userRepository;
            _userPhotoRepository = userPhotoRepository;
            _productAttributeDomainService = productAttributeDomainService;
            _farmEntityRepository = farmEntityRepository;
            _farmProductEntityRepository = farmProductEntityRepository;
            _productCategoryEntityRepository = productCategoryEntityRepository;
            _productPriceEntityRepository = productPriceEntityRepository;
            _productEntityRepository = productEntityRepository;
            _productPictureDomainService = productPictureDomainService;
            _productCategoryDomainService = productCategoryDomainService;
            _productCategoryRelationEntityRepository = productCategoryRelationEntityRepository;
            _productAttributeAppService = productAttributeAppService;
            _productFarmDomainService = productFarmDomainService;
            _productPriceDomainService = productPriceDomainService;
            _dbContext = dbContext;
        }

        #region get
        public async Task<ProductResult> FindAsync(IdRequestFilter<long> filter)
        {
            var existing = await _productRepository.FindAsync(filter.Id);
            if (existing == null)
            {
                return null;
            }

            if ((existing.StatusId == _deletedStatus && !filter.CanGetDeleted) || (existing.StatusId == _inactivedStatus && !filter.CanGetInactived))
            {
                return null;
            }

            return MapEntityToDto(existing);
        }

        public async Task<ProductResult> FindByNameAsync(string name)
        {
            var existing = await _productRepository.FindByNameAsync(name);
            if (existing == null)
            {
                return null;
            }

            if (existing.StatusId == _deletedStatus || existing.StatusId == _inactivedStatus)
            {
                return null;
            }

            return MapEntityToDto(existing);
        }

        public async Task<ProductResult> FindDetailAsync(IdRequestFilter<long> filter)
        {
            var product = await FindAsync(filter);
            if (product == null)
            {
                return null;
            }

            var farmQuery = from farm in _farmEntityRepository.Table
                            join farmProduct in _farmProductEntityRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            where farmProduct.ProductId == filter.Id
                            where (farm.StatusId == _farmDeletedStatus && filter.CanGetDeleted)
                                    || (farm.StatusId == _farmInactivedStatus && filter.CanGetInactived)
                                    || (farm.StatusId != _farmDeletedStatus && farm.StatusId != _farmInactivedStatus)
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var productCategoryQuery = from category in _productCategoryEntityRepository.Table
                                       join categoryRelation in _productCategoryRelationEntityRepository.Table
                                       on category.Id equals categoryRelation.ProductCategoryId
                                       where categoryRelation.ProductId == filter.Id
                                       select new
                                       {
                                           Id = categoryRelation.Id,
                                           CategoryId = category.Id,
                                           ProductId = categoryRelation.ProductId,
                                           Name = category.Name
                                       };

            var priceQuery = await _productPriceEntityRepository.GetAsync(x => x.IsCurrent && x.ProductId == filter.Id);
            var result = new ProductResult
            {
                Description = product.Description,
                CreatedDate = product.CreatedDate,
                CreatedById = product.CreatedById,
                Id = product.Id,
                Name = product.Name,
                UpdatedById = product.UpdatedById,
                UpdatedDate = product.UpdatedDate,
                Price = priceQuery?.FirstOrDefault()?.Price ?? 0,
                StatusId = product.StatusId,
                Categories = productCategoryQuery.Select(x => new ProductCategoryResult()
                {
                    Id = x.CategoryId,
                    Name = x.Name
                }),
                Farms = farmQuery.Select(x => new ProductFarmResult()
                {
                    Id = x.Id,
                    FarmId = x.FarmId,
                    Name = x.Name
                })
            };

            await PopulateProductPicturesAsync(product, filter);
            await PopulateProductAttributesAsync(product, filter);

            await PopulateModifiersAsync(product);
            return product;
        }

        private async Task PopulateModifiersAsync(ProductResult product)
        {
            product.CreatedBy = (await _userRepository.FindByIdAsync(product.CreatedById)).DisplayName;
            product.UpdatedBy = (await _userRepository.FindByIdAsync(product.CreatedById)).DisplayName;
        }

        private async Task PopulateProductPicturesAsync(ProductResult product, IdRequestFilter<long> filter)
        {
            var pictures = await _productPictureAppService.GetListByProductIdAsync(new IdRequestFilter<long>
            {
                Id = filter.Id,
                CanGetDeleted = filter.CanGetDeleted,
                CanGetInactived = filter.CanGetInactived
            });
            product.Pictures = pictures.Select(x => new PictureResult
            {
                Id = x.PictureId
            });
        }

        private async Task PopulateProductAttributesAsync(ProductResult product, IdRequestFilter<long> filter)
        {
            product.ProductAttributes = await _productAttributeAppService.GetAttributeRelationsByProductIdAsync(filter.Id);
        }

        public async Task<BasePageList<ProductResult>> GetAsync(ProductFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var productQuery = _productEntityRepository.Get(x => (x.StatusId == _deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == _inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != _deletedStatus && x.StatusId != _inactivedStatus));
            if (!string.IsNullOrEmpty(search))
            {
                productQuery = productQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.StatusId.HasValue)
            {
                productQuery = productQuery.Where(x => x.StatusId == filter.StatusId);
            }

            if (filter.UpdatedById.HasValue)
            {
                productQuery = productQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.CategoryId.HasValue)
            {
                productQuery = productQuery.Where(x => x.ProductCategoryRelations.Any(c => c.ProductCategoryId == filter.CategoryId));
            }

            if (filter.FarmId.HasValue)
            {
                productQuery = productQuery.Where(x => x.ProductFarmRelations.Any(c => c.FarmId == filter.FarmId));
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
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTimeOffset.UtcNow);
            }

            var filteredNumber = productQuery.Select(x => x.Id).Count();

            var farmQuery = from farm in _farmEntityRepository.Get(x => x.StatusId != _farmDeletedStatus)
                            join farmProduct in _farmProductEntityRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var query = from product in productQuery
                        join pr in _productPriceEntityRepository.Get(x => x.IsCurrent)
                        on product.Id equals pr.ProductId into prices
                        from price in prices.DefaultIfEmpty()
                        join fp in farmQuery
                        on product.Id equals fp.ProductId into farmProducts
                        select new ProductResult
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = price != null ? price.Price : 0,
                            CreatedById = product.CreatedById,
                            CreatedDate = product.CreatedDate,
                            Description = product.Description,
                            UpdatedById = product.UpdatedById,
                            UpdatedDate = product.UpdatedDate,
                            StatusId = product.StatusId,
                            Farms = farmProducts.Select(x => new ProductFarmResult
                            {
                                Id = x.Id,
                                FarmId = x.FarmId,
                                Name = x.Name
                            })
                        };

            var products = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            await PopulateDetailsAsync(products, filter);
            var result = new BasePageList<ProductResult>(products)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<IList<ProductResult>> GetRelevantsAsync(long id, ProductFilter filter)
        {
            var exist = (from pr in _productEntityRepository.Get(x => x.Id == id && x.StatusId != _deletedStatus)
                         join fp in _farmProductEntityRepository.Table
                         on pr.Id equals fp.ProductId into farmProducts
                         join productCategoryRelation in _productCategoryRelationEntityRepository.Table
                         on pr.Id equals productCategoryRelation.ProductId into categoriesRelation
                         select new ProductResult
                         {
                             Id = pr.Id,
                             CreatedById = pr.CreatedById,
                             UpdatedById = pr.UpdatedById,
                             Categories = categoriesRelation.Select(x => new ProductCategoryResult()
                             {
                                 Id = x.ProductCategoryId
                             }),
                             Farms = farmProducts.Select(x => new ProductFarmResult()
                             {
                                 FarmId = x.FarmId
                             })
                         }).FirstOrDefault();

            var farmIds = exist.Farms.Select(x => x.FarmId);
            var categoryIds = exist.Categories.Select(x => x.Id);

            var farmQuery = from farm in _farmEntityRepository.Get(x => farmIds.Contains(x.Id))
                            join farmProduct in _farmProductEntityRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var relevantProductQuery = (from pr in _productEntityRepository.Get(x => x.Id != exist.Id)
                                        join fp in farmQuery
                                        on pr.Id equals fp.ProductId into farmProducts

                                        join productCategoryRelation in _productCategoryRelationEntityRepository.Table
                                        on pr.Id equals productCategoryRelation.ProductId into categoriesRelation
                                        from categoryRelation in categoriesRelation.DefaultIfEmpty()

                                        join prc in _productPriceEntityRepository.Get(x => x.IsCurrent)
                                        on pr.Id equals prc.ProductId into prices
                                        from price in prices.DefaultIfEmpty()

                                        where pr.CreatedById == exist.CreatedById
                                        || categoryIds.Contains(categoryRelation.ProductCategoryId)
                                        || pr.UpdatedById == exist.UpdatedById
                                        select new ProductResult
                                        {
                                            Id = pr.Id,
                                            Name = pr.Name,
                                            Price = price != null ? price.Price : 0,
                                            CreatedById = pr.CreatedById,
                                            CreatedDate = pr.CreatedDate,
                                            Description = pr.Description,
                                            UpdatedById = pr.UpdatedById,
                                            UpdatedDate = pr.UpdatedDate,
                                            Farms = farmProducts.Select(x => new ProductFarmResult
                                            {
                                                Id = x.Id,
                                                FarmId = x.FarmId,
                                                Name = x.Name
                                            })
                                        });

            var relevantProducts = await relevantProductQuery
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            await PopulateDetailsAsync(relevantProducts, new ProductFilter
            {
                CanGetDeleted = true,
                CanGetInactived = true,
            });
            return relevantProducts;
        }

        private async Task PopulateDetailsAsync(IList<ProductResult> products, ProductFilter filter = null)
        {
            var createdByIds = products.Select(x => x.CreatedById).ToArray();
            var updatedByIds = products.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetByIdsAsync(updatedByIds);

            var productIds = products.Select(x => x.Id);
            var pictures = await _productPictureAppService.GetListByProductIdsAsync(productIds, new IdRequestFilter<long>
            {
                CanGetDeleted = filter == null || filter.CanGetDeleted,
                CanGetInactived = filter == null || filter.CanGetInactived
            }, ProductPictureTypes.Thumbnail);

            var userAvatars = await _userPhotoRepository.GetListByUserIdsAsync(createdByIds, UserPictureTypes.Avatar);
            foreach (var product in products)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == product.CreatedById);
                product.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == product.UpdatedById);
                product.UpdatedBy = updatedBy.DisplayName;

                var productPictures = pictures.Where(x => x.ProductId == product.Id);
                if (productPictures != null && productPictures.Any())
                {
                    product.Pictures = productPictures.Select(x => new PictureResult
                    {
                        Id = x.PictureId
                    });
                }

                var avatar = userAvatars.FirstOrDefault(x => x.UserId == product.CreatedById);
                if (avatar != null)
                {
                    product.CreatedByPhotoCode = avatar.Code;
                }
            }
        }

        public async Task<IList<ProductResult>> GetProductByCategoryIdAsync(IdRequestFilter<int> categoryIdFilter)
        {
            return await (from relation in _productCategoryRelationEntityRepository.Get(x => x.ProductCategoryId == categoryIdFilter.Id)
                          join product in _productEntityRepository.Get(x => (x.StatusId == _deletedStatus && categoryIdFilter.CanGetDeleted)
                                            || (x.StatusId == _inactivedStatus && categoryIdFilter.CanGetInactived)
                                            || (x.StatusId != _deletedStatus && x.StatusId != _inactivedStatus))
                          on relation.ProductId equals product.Id
                          select new ProductResult
                          {
                              Id = product.Id,
                              Name = product.Name,
                              CreatedById = product.CreatedById,
                              CreatedDate = product.CreatedDate,
                              Description = product.Description,
                              UpdatedById = product.UpdatedById,
                              UpdatedDate = product.UpdatedDate,
                          })
                .ToListAsync();
        }

        private ProductResult MapEntityToDto(Product entity)
        {
            return new ProductResult
            {
                CreatedDate = entity.CreatedDate,
                CreatedById = entity.CreatedById,
                Id = entity.Id,
                Name = entity.Name,
                UpdatedById = entity.UpdatedById,
                UpdatedDate = entity.UpdatedDate,
                StatusId = entity.StatusId
            };
        }
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(ProductModifyRequest request)
        {
            var id = await _productRepository.CreateAsync(new Product
            {
                CreatedById = request.CreatedById,
                UpdatedById = request.UpdatedById,
                Description = request.Description,
                Name = request.Name,
                StatusId = _pendingStatus
            });
            if (id <= 0)
            {
                return -1;
            }

            foreach (var category in request.Categories)
            {
                _productCategoryRelationEntityRepository.Insert(new ProductCategoryRelation()
                {
                    ProductCategoryId = category.Id,
                    ProductId = id
                });
            }

            var modifiedDate = DateTimeOffset.UtcNow;
            foreach (var farm in request.Farms)
            {
                _farmProductEntityRepository.Insert(new FarmProduct
                {
                    FarmId = farm.FarmId,
                    ProductId = id,
                    IsLinked = true,
                    LinkedById = request.CreatedById,
                    LinkedDate = modifiedDate
                });
            }

            _productPriceEntityRepository.Insert(new ProductPrice()
            {
                PricedDate = modifiedDate,
                ProductId = id,
                Price = request.Price,
                IsCurrent = true
            });

            if (request.Pictures.Any())
            {
                await _productPictureAppService.CreateAsync(new ProductPicturesModifyRequest
                {
                    CreatedById = request.CreatedById,
                    ProductId = id,
                    Pictures = request.Pictures,
                    UpdatedById = request.UpdatedById
                }, needSaveChanges: false);
            }

            if (request.ProductAttributes == null || !request.ProductAttributes.Any())
            {
                return id;
            }

            var groupOfAttributes = new List<ProductAttributeRelationRequest>();
            foreach (var attributeRelation in request.ProductAttributes)
            {
                var existAttribute = groupOfAttributes.FirstOrDefault(x => x.ProductAttributeId == attributeRelation.ProductAttributeId);
                if (existAttribute != null)
                {
                    var attributeValues = existAttribute.AttributeRelationValues.ToList();
                    attributeValues.AddRange(attributeRelation.AttributeRelationValues);
                    existAttribute.AttributeRelationValues = attributeValues;
                }
                else
                {
                    groupOfAttributes.Add(attributeRelation);
                }
            }

            foreach (var attributeRelation in groupOfAttributes)
            {
                attributeRelation.ProductId = id;
                await _productAttributeDomainService.CreateRelationAsync(MapAttributeRelationDtoToEntity(attributeRelation));
            }

            await _dbContext.SaveChangesAsync();
            return id;
        }

        public async Task<bool> UpdateAsync(ProductModifyRequest request)
        {
            var existing = await _productRepository.FindAsync(request.Id);
            existing.Description = request.Description;
            existing.Name = request.Name;
            existing.UpdatedById = request.UpdatedById;
            
            var isUpdated = await _productRepository.UpdateAsync(existing);
            if (!isUpdated)
            {
                return false;
            }

            var modifiedDate = DateTimeOffset.UtcNow;
            // Update Category
            var categoryIds = request.Categories.Select(x => x.Id).ToList();
            await _productCategoryDomainService.UpdateProductCategoryRelationsAsync(request.Id, categoryIds);

            // Update Farm
            var farmIds = request.Farms.Select(x => x.FarmId).ToList();
            await _productFarmDomainService.UpdateProductFarmRelationsAsync(request.Id, farmIds, request.UpdatedById);

            // Unlink all price
            await _productPriceDomainService.UpdateProductPriceAsync(request.Id, request.Price);

            await _productPictureAppService.UpdateAsync(new ProductPicturesModifyRequest
            {
                ProductId = request.Id,
                CreatedById = request.CreatedById,
                UpdatedById = request.UpdatedById,
                Pictures = request.Pictures
            });

            // Delete all product attributes in cases no product attributes from the request
            if (request.ProductAttributes == null || !request.ProductAttributes.Any())
            {
                await _productAttributeDomainService.DeleteRelationByProductIdAsync(request.Id);
                return isUpdated;
            }

            var productAttributeIds = request.ProductAttributes.Where(x => x.Id != 0).Select(x => x.Id);
            await _productAttributeDomainService.DeleteRelationNotInIdsAsync(request.Id, productAttributeIds);

            foreach (var attributeRelation in request.ProductAttributes)
            {
                attributeRelation.ProductId = request.Id;
                var isAttributeRelationExist = attributeRelation.Id != 0 && await _productAttributeDomainService.IsAttributeRelationExistAsync(attributeRelation.Id);
                if (!isAttributeRelationExist)
                {
                    await _productAttributeDomainService.CreateRelationAsync(MapAttributeRelationDtoToEntity(attributeRelation));
                }
                else
                {
                    await _productAttributeDomainService.UpdateRelationAsync(attributeRelation.Id, MapAttributeRelationDtoToEntity(attributeRelation));
                }
            }

            await _dbContext.SaveChangesAsync();
            return isUpdated;
        }

        private ProductAttributeRelation MapAttributeRelationDtoToEntity(ProductAttributeRelationRequest attributeRelation)
        {
            return new ProductAttributeRelation
            {
                Id = attributeRelation.Id,
                AttributeControlTypeId = attributeRelation.ControlTypeId,
                DisplayOrder = attributeRelation.DisplayOrder,
                IsRequired = attributeRelation.IsRequired,
                ProductAttributeId = attributeRelation.ProductAttributeId,
                ProductId = attributeRelation.ProductId,
                TextPrompt = attributeRelation.TextPrompt,
                ProductAttributeRelationValues = attributeRelation.AttributeRelationValues.Select(attributeValue => new ProductAttributeRelationValue
                {
                    Name = attributeValue.Name,
                    PriceAdjustment = attributeValue.PriceAdjustment,
                    PricePercentageAdjustment = attributeValue.PricePercentageAdjustment,
                    Quantity = attributeValue.Quantity,
                    DisplayOrder = attributeValue.DisplayOrder,
                }).ToList()
            };
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _productPictureDomainService.DeleteByProductIdAsync(id);
            await _productAttributeDomainService.DeleteRelationByProductIdAsync(id);
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(long productId, long updatedById)
        {
            await _productPictureDomainService.UpdateStatusByProductIdAsync(productId, updatedById, PictureStatuses.Deleted);

            var existing = await _productRepository.FindAsync(productId);
            existing.StatusId = _deletedStatus;
            existing.UpdatedById = updatedById;
            return await _productRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeactiveAsync(long productId, long updatedById)
        {
            await _productPictureDomainService.UpdateStatusByProductIdAsync(productId, updatedById, PictureStatuses.Inactived);
            var existing = await _productRepository.FindAsync(productId);
            existing.StatusId = _inactivedStatus;
            existing.UpdatedById = updatedById;
            return await _productRepository.UpdateAsync(existing);
        }

        public async Task<bool> ActiveAsync(long productId, long updatedById)
        {
            await _productPictureDomainService.UpdateStatusByProductIdAsync(productId, updatedById, PictureStatuses.Actived);
            var existing = await _productRepository.FindAsync(productId);
            existing.StatusId = _activedStatus;
            existing.UpdatedById = updatedById;
            return await _productRepository.UpdateAsync(existing);
        }
        #endregion

        #region product pictures
        public async Task<BasePageList<ProductPictureResult>> GetPicturesAsync(ProductPictureFilter filter)
        {
            var productPicturesPageList = await _productPictureAppService.GetAsync(filter);
            var createdByIds = productPicturesPageList.Collections.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);
            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);

            foreach (var productPicture in productPicturesPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == productPicture.PictureCreatedById);
                productPicture.PictureCreatedBy = createdBy.DisplayName;
            }

            return productPicturesPageList;
        }
        #endregion

        #region product status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (ProductStatuses)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<ProductStatuses>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion
    }
}
