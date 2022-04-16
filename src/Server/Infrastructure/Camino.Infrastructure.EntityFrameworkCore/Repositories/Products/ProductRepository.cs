using Camino.Shared.Requests.Filters;
using Camino.Core.Contracts.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using Camino.Shared.Results.Products;
using Camino.Core.Domain.Products;
using Camino.Core.Domain.Farms;
using Camino.Shared.Requests.Products;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Products
{
    public class ProductRepository : IProductRepository, IScopedDependency
    {
        private readonly IEntityRepository<Product> _productRepository;
        private readonly IEntityRepository<FarmProduct> _farmProductRepository;
        private readonly IEntityRepository<Farm> _farmRepository;
        private readonly IEntityRepository<ProductPrice> _productPriceRepository;
        private readonly IEntityRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IEntityRepository<ProductCategory> _productCategoryRepository;
        private readonly IAppDbContext _dbContext;

        public ProductRepository(IEntityRepository<Product> productRepository,
            IEntityRepository<ProductCategoryRelation> productCategoryRelationRepository,
            IEntityRepository<ProductPrice> productPriceRepository, IEntityRepository<FarmProduct> farmProductRepository,
            IEntityRepository<Farm> farmRepository, IEntityRepository<ProductCategory> productCategoryRepository,
            IAppDbContext dbContext)
        {
            _productRepository = productRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _productPriceRepository = productPriceRepository;
            _farmProductRepository = farmProductRepository;
            _farmRepository = farmRepository;
            _productCategoryRepository = productCategoryRepository;
            _dbContext = dbContext;
        }

        public async Task<ProductResult> FindAsync(IdRequestFilter<long> filter)
        {
            var deletedStatus = ProductStatus.Deleted.GetCode();
            var inactivedStatus = ProductStatus.Inactived.GetCode();
            var exist = await (from product in _productRepository
                               .Get(x => x.Id == filter.Id)
                               .Where(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                    || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                    || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
                               select new ProductResult
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

        public async Task<ProductResult> FindDetailAsync(IdRequestFilter<long> filter)
        {
            var farmDeletedStatus = ProductStatus.Deleted.GetCode();
            var farmInactivedStatus = ProductStatus.Inactived.GetCode();
            var farmQuery = from farm in _farmRepository
                            .Get(x => x.Id == filter.Id)
                            join farmProduct in _farmProductRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            where (farm.StatusId == farmDeletedStatus && filter.CanGetDeleted)
                                    || (farm.StatusId == farmInactivedStatus && filter.CanGetInactived)
                                    || (farm.StatusId != farmDeletedStatus && farm.StatusId != farmInactivedStatus)
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

            var deletedStatus = ProductStatus.Deleted.GetCode();
            var inactivedStatus = ProductStatus.Inactived.GetCode();
            var product = await (from p in _productRepository
                                 .Get(x => x.Id == filter.Id)
                                 join pr in _productPriceRepository.Get(x => x.IsCurrent)
                                 on p.Id equals pr.ProductId into prices
                                 from price in prices.DefaultIfEmpty()

                                 join categoryRelation in productCategoryQuery
                                 on p.Id equals categoryRelation.ProductId into productCategories

                                 join fp in farmQuery
                                 on p.Id equals fp.ProductId into farmProducts
                                 where (p.StatusId == deletedStatus && filter.CanGetDeleted)
                                     || (p.StatusId == inactivedStatus && filter.CanGetInactived)
                                     || (p.StatusId != deletedStatus && p.StatusId != inactivedStatus)
                                 select new ProductResult
                                 {
                                     Description = p.Description,
                                     CreatedDate = p.CreatedDate,
                                     CreatedById = p.CreatedById,
                                     Id = p.Id,
                                     Name = p.Name,
                                     UpdatedById = p.UpdatedById,
                                     UpdatedDate = p.UpdatedDate,
                                     Price = price.Price,
                                     StatusId = p.StatusId,
                                     Categories = productCategories.Select(x => new ProductCategoryResult()
                                     {
                                         Id = x.CategoryId,
                                         Name = x.Name
                                     }),
                                     Farms = farmProducts.Select(x => new ProductFarmResult()
                                     {
                                         Id = x.Id,
                                         FarmId = x.FarmId,
                                         Name = x.Name
                                     })
                                 }).FirstOrDefaultAsync();
            return product;
        }

        public ProductResult FindByName(string name)
        {
            var exist = _productRepository.Get(x => x.Name == name && x.StatusId != ProductStatus.Deleted.GetCode())
                .FirstOrDefault();

            var product = new ProductResult()
            {
                CreatedDate = exist.CreatedDate,
                CreatedById = exist.CreatedById,
                Id = exist.Id,
                Name = exist.Name,
                UpdatedById = exist.UpdatedById,
                UpdatedDate = exist.UpdatedDate,
                Description = exist.Description,
            };

            return product;
        }

        public async Task<BasePageList<ProductResult>> GetAsync(ProductFilter filter)
        {
            var deletedStatus = ProductStatus.Deleted.GetCode();
            var inactivedStatus = ProductStatus.Inactived.GetCode();
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var productQuery = _productRepository.Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus));
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

            var farmQuery = from farm in _farmRepository.Get(x => x.StatusId != ProductStatus.Deleted.GetCode())
                            join farmProduct in _farmProductRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var query = from product in productQuery
                        join pr in _productPriceRepository.Get(x => x.IsCurrent)
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

            var result = new BasePageList<ProductResult>(products)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<IList<ProductResult>> GetRelevantsAsync(long id, ProductFilter filter)
        {
            var exist = (from pr in _productRepository.Get(x => x.Id == id && x.StatusId != ProductStatus.Deleted.GetCode())
                         join fp in _farmProductRepository.Table
                         on pr.Id equals fp.ProductId into farmProducts
                         join productCategoryRelation in _productCategoryRelationRepository.Table
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

            var relevantProductQuery = (from pr in _productRepository.Get(x => x.Id != exist.Id)
                                        join fp in farmQuery
                                        on pr.Id equals fp.ProductId into farmProducts

                                        join productCategoryRelation in _productCategoryRelationRepository.Table
                                        on pr.Id equals productCategoryRelation.ProductId into categoriesRelation
                                        from categoryRelation in categoriesRelation.DefaultIfEmpty()

                                        join prc in _productPriceRepository.Get(x => x.IsCurrent)
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

            return relevantProducts;
        }

        public async Task<long> CreateAsync(ProductModifyRequest request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var newProduct = new Product()
            {
                CreatedById = request.CreatedById,
                UpdatedById = request.UpdatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Description = request.Description,
                Name = request.Name,
                StatusId = ProductStatus.Pending.GetCode()
            };

            await _productRepository.InsertAsync(newProduct);
            await _dbContext.SaveChangesAsync();
            if (newProduct.Id > 0)
            {
                foreach (var category in request.Categories)
                {
                    _productCategoryRelationRepository.Insert(new ProductCategoryRelation()
                    {
                        ProductCategoryId = category.Id,
                        ProductId = newProduct.Id
                    });
                }

                foreach (var farm in request.Farms)
                {
                    _farmProductRepository.Insert(new FarmProduct()
                    {
                        FarmId = farm.FarmId,
                        ProductId = newProduct.Id,
                        IsLinked = true,
                        LinkedById = request.CreatedById,
                        LinkedDate = modifiedDate
                    });
                }

                _productPriceRepository.Insert(new ProductPrice()
                {
                    PricedDate = modifiedDate,
                    ProductId = newProduct.Id,
                    Price = request.Price,
                    IsCurrent = true
                });
                await _dbContext.SaveChangesAsync();
            }

            return newProduct.Id;
        }

        public async Task<bool> UpdateAsync(ProductModifyRequest request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var product = _productRepository.Find(x => x.Id == request.Id);
            product.Description = request.Description;
            product.Name = request.Name;
            product.UpdatedById = request.UpdatedById;
            product.UpdatedDate = modifiedDate;

            // Update Category
            var categoryIds = request.Categories.Select(x => x.Id);
            await _productCategoryRelationRepository
                        .DeleteAsync(x => x.ProductId == request.Id && !categoryIds.Contains(x.ProductCategoryId));

            var linkedCategoryIds = _productCategoryRelationRepository
                .Get(x => x.ProductId == request.Id && categoryIds.Contains(x.ProductCategoryId))
                .Select(x => x.ProductCategoryId)
                .ToList();

            var unlinkedCategories = request.Categories.Where(x => !linkedCategoryIds.Contains(x.Id));
            if (unlinkedCategories != null && unlinkedCategories.Any())
            {
                foreach (var category in unlinkedCategories)
                {
                    _productCategoryRelationRepository.Insert(new ProductCategoryRelation()
                    {
                        ProductCategoryId = category.Id,
                        ProductId = request.Id
                    });
                }

                await _dbContext.SaveChangesAsync();
            }

            // Update Farm
            var farmIds = request.Farms.Select(x => x.FarmId);
            await _farmProductRepository
                        .DeleteAsync(x => x.ProductId == request.Id && !farmIds.Contains(x.FarmId));

            var linkedFarmIds = _farmProductRepository
                .Get(x => x.ProductId == request.Id && farmIds.Contains(x.FarmId))
                .Select(x => x.FarmId)
                .ToList();

            var unlinkedFarms = request.Farms.Where(x => !linkedFarmIds.Contains(x.FarmId));
            if (unlinkedFarms != null && unlinkedFarms.Any())
            {
                foreach (var farm in unlinkedFarms)
                {
                    _farmProductRepository.Insert(new FarmProduct()
                    {
                        FarmId = farm.FarmId,
                        ProductId = request.Id,
                        IsLinked = true,
                        LinkedById = product.CreatedById,
                        LinkedDate = modifiedDate
                    });
                }

                await _dbContext.SaveChangesAsync();
            }

            // Unlink all price
            await _productPriceRepository.Get(x => x.ProductId == request.Id && x.IsCurrent && x.Price != request.Price)
                .SetEntry(x => x.IsCurrent, false)
                .UpdateAsync();

            await _productPriceRepository.InsertAsync(new ProductPrice()
            {
                PricedDate = modifiedDate,
                ProductId = request.Id,
                Price = request.Price,
                IsCurrent = true
            });

            await _productRepository.UpdateAsync(product);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IList<ProductResult>> GetProductByCategoryIdAsync(IdRequestFilter<int> categoryIdFilter)
        {
            var deletedStatus = ProductStatus.Deleted.GetCode();
            var inactivedStatus = ProductStatus.Inactived.GetCode();
            return await (from relation in _productCategoryRelationRepository.Get(x => x.ProductCategoryId == categoryIdFilter.Id)
                          join product in _productRepository.Get(x => (x.StatusId == deletedStatus && categoryIdFilter.CanGetDeleted)
                                            || (x.StatusId == inactivedStatus && categoryIdFilter.CanGetInactived)
                                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
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

        public async Task<bool> DeleteAsync(long id)
        {
            await _farmProductRepository.DeleteAsync(x => x.ProductId == id);

            await _productPriceRepository.DeleteAsync(x => x.ProductId == id);

            await _productCategoryRelationRepository.DeleteAsync(x => x.ProductId == id);

            await _productRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(ProductModifyRequest request)
        {
            await _productRepository.Get(x => x.Id == request.Id)
                .SetEntry(x => x.StatusId, (int)ProductStatus.Deleted)
                .SetEntry(x => x.UpdatedById, request.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeactiveAsync(ProductModifyRequest request)
        {
            await _productRepository.Get(x => x.Id == request.Id)
                .SetEntry(x => x.StatusId, (int)ProductStatus.Inactived)
                .SetEntry(x => x.UpdatedById, request.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> ActiveAsync(ProductModifyRequest request)
        {
            await _productRepository.Get(x => x.Id == request.Id)
                .SetEntry(x => x.StatusId, (int)ProductStatus.Actived)
                .SetEntry(x => x.UpdatedById, request.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }
    }
}
