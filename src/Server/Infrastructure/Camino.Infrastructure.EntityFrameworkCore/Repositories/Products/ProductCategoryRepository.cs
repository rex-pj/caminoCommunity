using Camino.Core.Contracts.Data;
using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using Camino.Core.Domain.Products;
using Camino.Shared.Requests.Products;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Products
{
    public class ProductCategoryRepository : IProductCategoryRepository, IScopedDependency
    {
        private readonly IEntityRepository<ProductCategory> _productCategoryRepository;
        private readonly IAppDbContext _dbContext;

        public ProductCategoryRepository(IEntityRepository<ProductCategory> productCategoryRepository, IAppDbContext dbContext)
        {
            _productCategoryRepository = productCategoryRepository;
            _dbContext = dbContext;
        }

        public async Task<ProductCategoryResult> FindAsync(int id)
        {
            var category = await (from child in _productCategoryRepository.Table
                                  join parent in _productCategoryRepository.Table
                                  on child.ParentId equals parent.Id into categories
                                  from cate in categories.DefaultIfEmpty()
                                  where child.Id == id
                                  select new ProductCategoryResult
                                  {
                                      Description = child.Description,
                                      CreatedDate = child.CreatedDate,
                                      CreatedById = child.CreatedById,
                                      Id = child.Id,
                                      Name = child.Name,
                                      ParentId = child.ParentId,
                                      UpdatedById = child.UpdatedById,
                                      UpdatedDate = child.UpdatedDate,
                                      StatusId = child.StatusId,
                                      ParentCategoryName = cate != null ? cate.Name : null
                                  }).FirstOrDefaultAsync();
            return category;
        }

        public ProductCategoryResult FindByName(string name)
        {
            var category = _productCategoryRepository.Get(x => x.Name == name)
                .Select(x => new ProductCategoryResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    StatusId = x.StatusId,
                    Description = x.Description
                })
                .FirstOrDefault();

            return category;
        }

        public async Task<BasePageList<ProductCategoryResult>> GetAsync(ProductCategoryFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var categoryQuery = _productCategoryRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                categoryQuery = categoryQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.StatusId.HasValue)
            {
                categoryQuery = categoryQuery.Where(x => x.StatusId == filter.StatusId);
            }

            if (filter.CreatedById.HasValue)
            {
                categoryQuery = categoryQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                categoryQuery = categoryQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                categoryQuery = categoryQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                categoryQuery = categoryQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                categoryQuery = categoryQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var query = categoryQuery.Select(a => new ProductCategoryResult
            {
                CreatedById = a.CreatedById,
                CreatedDate = a.CreatedDate,
                Description = a.Description,
                Id = a.Id,
                Name = a.Name,
                ParentId = a.ParentId,
                UpdatedById = a.UpdatedById,
                UpdatedDate = a.UpdatedDate,
                StatusId = a.StatusId
            });

            var filteredNumber = query.Select(x => x.Id).Count();

            var categories = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ProductCategoryResult>(categories)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public List<ProductCategoryResult> Get(Expression<Func<ProductCategory, bool>> filter)
        {
            var categories = _productCategoryRepository.Get(filter).Select(a => new ProductCategoryResult
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            return categories;
        }

        public async Task<IList<ProductCategoryResult>> SearchParentsAsync(BaseFilter filter, int[] currentIds)
        {
            var keyword = string.IsNullOrEmpty(filter.Keyword) ? filter.Keyword.ToLower() : "";
            var query = _productCategoryRepository.Get(x => !x.ParentId.HasValue)
                .Select(c => new ProductCategoryResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentId = c.ParentId
                });

            if (currentIds != null && currentIds.Any())
            {
                query = query.Where(x => !currentIds.Contains(x.Id));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword));
            }

            if (filter.PageSize > 0)
            {
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var categories = await query
                .Select(x => new ProductCategoryResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId
                })
                .ToListAsync();

            return categories;
        }

        public async Task<IList<ProductCategoryResult>> SearchAsync(BaseFilter filter, int[] currentIds)
        {
            var keyword = string.IsNullOrEmpty(filter.Keyword) ? filter.Keyword.ToLower() : "";
            var queryParents = _productCategoryRepository.Get(x => !x.ParentId.HasValue);

            var queryChildrens = _productCategoryRepository.Get(x => x.ParentId.HasValue);
            if (currentIds != null && currentIds.Any())
            {
                queryParents = queryParents.Where(x => !currentIds.Contains(x.Id));
                queryChildrens = queryChildrens.Where(x => !currentIds.Contains(x.Id));
            }

            var query = from parent in queryParents
                        join child in queryChildrens
                        on parent.Id equals child.ParentId into joined
                        from j in joined.DefaultIfEmpty()
                        orderby j.ParentId
                        select new ProductCategoryResult
                        {
                            Id = j.Id,
                            Name = j.Name,
                            Description = j.Description,
                            ParentId = j.ParentId,
                            ParentCategory = new ProductCategoryResult()
                            {
                                Id = parent.Id,
                                Name = parent.Name,
                                Description = parent.Description,
                            }
                        };

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.ToLower().Contains(keyword)
                        || x.Description.ToLower().Contains(keyword)
                        || x.ParentCategory.Name.ToLower().Contains(keyword)
                        || x.ParentCategory.Description.ToLower().Contains(keyword));
            }

            if (filter.PageSize > 0)
            {
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var data = await query
                .Select(x => new ProductCategoryResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    ParentCategory = new ProductCategoryResult()
                    {
                        Id = x.ParentCategory.Id,
                        Name = x.ParentCategory.Name,
                        Description = x.ParentCategory.Description,
                    }
                })
                .ToListAsync();

            var categories = new List<ProductCategoryResult>();
            foreach (var category in data)
            {
                if (!categories.Any(x => x.Id == category.ParentId))
                {
                    categories.Add(category.ParentCategory);
                }

                if (category.Id != 0)
                {
                    categories.Add(category);
                }
            }

            return categories;
        }

        public async Task<int> CreateAsync(ProductCategoryRequest category)
        {
            var newCategory = new ProductCategory()
            {
                Description = category.Description,
                Name = category.Name,
                ParentId = category.ParentId,
                CreatedById = category.CreatedById,
                UpdatedById = category.UpdatedById,
                UpdatedDate = DateTimeOffset.UtcNow,
                CreatedDate = DateTimeOffset.UtcNow,
                StatusId = ProductCategoryStatus.Actived.GetCode()
            };

            await _productCategoryRepository.InsertAsync(newCategory);
            await _dbContext.SaveChangesAsync();
            return newCategory.Id;
        }

        public async Task<bool> UpdateAsync(ProductCategoryRequest category)
        {
            var existing = await _productCategoryRepository.FindAsync(x => x.Id == category.Id);
            existing.Description = category.Description;
            existing.Name = category.Name;
            existing.ParentId = category.ParentId;
            existing.UpdatedById = category.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeactivateAsync(ProductCategoryRequest request)
        {
            var existing = await _productCategoryRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = (int)ProductCategoryStatus.Inactived;
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ActiveAsync(ProductCategoryRequest request)
        {
            var existing = await _productCategoryRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = (int)ProductCategoryStatus.Actived;
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _productCategoryRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return deletedNumbers > 0;
        }
    }
}
