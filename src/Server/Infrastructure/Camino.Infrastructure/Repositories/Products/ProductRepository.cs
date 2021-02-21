using Camino.Shared.Requests.Filters;
using Camino.Core.Contracts.Data;
using LinqToDB;
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

namespace Camino.Service.Repository.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<FarmProduct> _farmProductRepository;
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<ProductPrice> _productPriceRepository;
        private readonly IRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;

        public ProductRepository(IRepository<Product> productRepository,
            IRepository<ProductCategoryRelation> productCategoryRelationRepository,
            IRepository<ProductPrice> productPriceRepository, IRepository<FarmProduct> farmProductRepository, 
            IRepository<Farm> farmRepository, IRepository<ProductCategory> productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _productPriceRepository = productPriceRepository;
            _farmProductRepository = farmProductRepository;
            _farmRepository = farmRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<ProductResult> FindAsync(long id)
        {
            var exist = await (from product in _productRepository.Get(x => x.Id == id && !x.IsDeleted)
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

        public async Task<ProductResult> FindDetailAsync(long id)
        {
            var farmQuery = from farm in _farmRepository.Get(x => !x.IsDeleted)
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

            var product = await (from p in _productRepository.Get(x => !x.IsDeleted && x.Id == id)
                                 join pr in _productPriceRepository.Get(x => x.IsCurrent)
                                 on p.Id equals pr.ProductId into prices
                                 from price in prices.DefaultIfEmpty()

                                 join categoryRelation in productCategoryQuery
                                 on p.Id equals categoryRelation.ProductId into productCategories

                                 join fp in farmQuery
                                 on p.Id equals fp.ProductId into farmProducts
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
            var exist = _productRepository.Get(x => x.Name == name && !x.IsDeleted)
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
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var productQuery = _productRepository.Get(x => !x.IsDeleted);
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

            var farmQuery = from farm in _farmRepository.Get(x => !x.IsDeleted)
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
            var exist = (from pr in _productRepository.Get(x => x.Id == id && !x.IsDeleted)
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
                IsPublished = true
            };

            var id = await _productRepository.AddWithInt64EntityAsync(newProduct);
            if (id > 0)
            {
                foreach (var category in request.Categories)
                {
                    _productCategoryRelationRepository.Add(new ProductCategoryRelation()
                    {
                        ProductCategoryId = category.Id,
                        ProductId = id
                    });
                }

                foreach (var farm in request.Farms)
                {
                    _farmProductRepository.Add(new FarmProduct()
                    {
                        FarmId = farm.FarmId,
                        ProductId = id,
                        IsLinked = true,
                        LinkedById = request.CreatedById,
                        LinkedDate = modifiedDate
                    });
                }

                _productPriceRepository.Add(new ProductPrice()
                {
                    PricedDate = modifiedDate,
                    ProductId = id,
                    Price = request.Price,
                    IsCurrent = true
                });
            }

            return id;
        }

        public async Task<bool> UpdateAsync(ProductModifyRequest request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var product = _productRepository.FirstOrDefault(x => x.Id == request.Id);
            product.Description = request.Description;
            product.Name = request.Name;
            product.UpdatedById = request.UpdatedById;
            product.UpdatedDate = modifiedDate;

            // Update Category
            var categoryIds = request.Categories.Select(x => x.Id);
            await _productCategoryRelationRepository
                        .Get(x => x.ProductId == request.Id && !categoryIds.Contains(x.ProductCategoryId))
                        .DeleteAsync();

            var linkedCategoryIds = _productCategoryRelationRepository
                .Get(x => x.ProductId == request.Id && categoryIds.Contains(x.ProductCategoryId))
                .Select(x => x.ProductCategoryId)
                .ToList();

            var unlinkedCategories = request.Categories.Where(x => !linkedCategoryIds.Contains(x.Id));
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
            var farmIds = request.Farms.Select(x => x.FarmId);
            await _farmProductRepository
                        .Get(x => x.ProductId == request.Id && !farmIds.Contains(x.FarmId))
                        .DeleteAsync();

            var linkedFarmIds = _farmProductRepository
                .Get(x => x.ProductId == request.Id && farmIds.Contains(x.FarmId))
                .Select(x => x.FarmId)
                .ToList();

            var unlinkedFarms = request.Farms.Where(x => !linkedFarmIds.Contains(x.FarmId));
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
            var totalPriceUpdated = await _productPriceRepository.Get(x => x.ProductId == request.Id && x.IsCurrent && x.Price != request.Price)
                .Set(x => x.IsCurrent, false)
                .UpdateAsync();

            if (totalPriceUpdated > 0)
            {
                await _productPriceRepository.AddAsync(new ProductPrice()
                {
                    PricedDate = modifiedDate,
                    ProductId = request.Id,
                    Price = request.Price,
                    IsCurrent = true
                });
            }

            _productRepository.Update(product);

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _farmProductRepository.Get(x => x.ProductId == id)
                .DeleteAsync();

            await _productPriceRepository.Get(x => x.ProductId == id)
                .DeleteAsync();

            await _productCategoryRelationRepository.Get(x => x.ProductId == id)
                .DeleteAsync();

            await _productRepository.Get(x => x.Id == id)
                .DeleteAsync();

            return true;
        }

        public async Task<bool> SoftDeleteAsync(long id)
        {
            await _productRepository.Get(x => x.Id == id)
                .Set(x => x.IsDeleted, true)
                .UpdateAsync();

            return true;
        }
    }
}
