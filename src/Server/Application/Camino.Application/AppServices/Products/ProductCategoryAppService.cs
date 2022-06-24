using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Core.DependencyInjection;
using Camino.Application.Contracts.AppServices.Products.Dtos;
using Camino.Application.Contracts;
using Camino.Shared.Enums;
using Camino.Application.Contracts.Utils;
using Camino.Shared.Exceptions;
using Camino.Core.Domains.Products;
using Camino.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Camino.Shared.Utils;

namespace Camino.Application.AppServices.Products
{
    public class ProductCategoryAppService : IProductCategoryAppService, IScopedDependency
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEntityRepository<ProductCategory> _productCategoryEntityRepository;

        public ProductCategoryAppService(IProductCategoryRepository productCategoryRepository, IUserRepository userRepository,
            IEntityRepository<ProductCategory> productCategoryEntityRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _userRepository = userRepository;
            _productCategoryEntityRepository = productCategoryEntityRepository;
        }

        #region get
        public async Task<ProductCategoryResult> FindAsync(long id)
        {
            var existing = await _productCategoryRepository.FindAsync(id);
            if (existing == null)
            {
                return null;
            }

            var category = new ProductCategoryResult
            {
                Description = existing.Description,
                CreatedDate = existing.CreatedDate,
                CreatedById = existing.CreatedById,
                Id = existing.Id,
                Name = existing.Name,
                ParentId = existing.ParentId,
                UpdatedById = existing.UpdatedById,
                UpdatedDate = existing.UpdatedDate,
                StatusId = existing.StatusId,
                ParentCategoryName = existing.ParentCategory?.Name
            };

            var createdByUser = await _userRepository.FindByIdAsync(category.CreatedById);
            var updatedByUser = await _userRepository.FindByIdAsync(category.UpdatedById);
            category.CreatedBy = createdByUser.DisplayName;
            category.UpdatedBy = updatedByUser.DisplayName;

            return category;
        }

        public async Task<ProductCategoryResult> FindByNameAsync(string name)
        {
            var existing = await _productCategoryRepository.FindByNameAsync(name);
            if (existing == null)
            {
                return null;
            }

            return new ProductCategoryResult()
            {
                Id = existing.Id,
                Name = existing.Name,
                StatusId = existing.StatusId,
                Description = existing.Description
            };
        }

        public async Task<BasePageList<ProductCategoryResult>> GetAsync(ProductCategoryFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var categoryQuery = _productCategoryEntityRepository.Table;
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

            await PopulateDetailsAsync(categories);
            var result = new BasePageList<ProductCategoryResult>(categories)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task PopulateDetailsAsync(IList<ProductCategoryResult> productCategories)
        {
            var createdByIds = productCategories.Select(x => x.CreatedById).ToArray();
            var updatedByIds = productCategories.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetByIdsAsync(updatedByIds);

            foreach (var category in productCategories)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }
        }

        public async Task<IList<ProductCategoryResult>> SearchParentsAsync(BaseFilter filter, long[] currentIds)
        {
            var keyword = string.IsNullOrEmpty(filter.Keyword) ? filter.Keyword.ToLower() : "";
            var query = _productCategoryEntityRepository.Get(x => !x.ParentId.HasValue)
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

        public async Task<IList<ProductCategoryResult>> SearchAsync(BaseFilter filter, long[] currentIds)
        {
            var keyword = string.IsNullOrEmpty(filter.Keyword) ? filter.Keyword.ToLower() : "";
            var queryParents = _productCategoryEntityRepository.Get(x => !x.ParentId.HasValue);

            var queryChildrens = _productCategoryEntityRepository.Get(x => x.ParentId.HasValue);
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
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(ProductCategoryRequest request)
        {
            var newCategory = new ProductCategory()
            {
                Description = request.Description,
                Name = request.Name,
                ParentId = request.ParentId,
                CreatedById = request.CreatedById,
                UpdatedById = request.UpdatedById,
                StatusId = ProductCategoryStatuses.Actived.GetCode()
            };
            return await _productCategoryRepository.CreateAsync(newCategory);
        }

        public async Task<bool> UpdateAsync(ProductCategoryRequest request)
        {
            var existing = await _productCategoryRepository.FindAsync(request.Id);
            existing.Description = request.Description;
            existing.Name = request.Name;
            existing.ParentId = request.ParentId;
            existing.UpdatedById = request.UpdatedById;
            return await _productCategoryRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeactivateAsync(ProductCategoryRequest request)
        {
            var existing = await _productCategoryRepository.FindAsync(request.Id);
            existing.StatusId = (int)ProductCategoryStatuses.Inactived;
            existing.UpdatedById = request.UpdatedById;
            return await _productCategoryRepository.UpdateAsync(existing);
        }

        public async Task<bool> ActiveAsync(ProductCategoryRequest request)
        {
            var existing = await _productCategoryRepository.FindAsync(request.Id);
            existing.StatusId = (int)ProductCategoryStatuses.Actived;
            existing.UpdatedById = request.UpdatedById;
            return await _productCategoryRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var hasProducts = await _productCategoryRepository.HasProductsAsync(id);
            if (hasProducts)
            {
                throw new CaminoApplicationException($"Some Products belong to this category need to be deleted or move to another category");
            }

            return await _productCategoryRepository.DeleteAsync(id);
        }
        #endregion

        #region category status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (ProductCategoryStatuses)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<ProductCategoryStatuses>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion
    }
}
