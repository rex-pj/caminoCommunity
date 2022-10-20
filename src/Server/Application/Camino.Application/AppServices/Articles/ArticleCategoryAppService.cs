using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domains;
using Camino.Core.Domains.Articles;
using Camino.Core.Domains.Articles.Repositories;
using Camino.Shared.Exceptions;
using Camino.Application.Contracts;
using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Application.Contracts.AppServices.Articles;
using Camino.Core.DependencyInjection;
using Camino.Application.Contracts.Utils;
using Microsoft.EntityFrameworkCore;
using Camino.Shared.Utils;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.AppServices.Articles
{
    public class ArticleCategoryAppService : IArticleCategoryAppService, IScopedDependency
    {
        private readonly IEntityRepository<Article> _articleEntityRepository;
        private readonly IEntityRepository<ArticleCategory> _articleCategoryEntityRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly int _inactivedStatus = ArticleCategoryStatuses.Inactived.GetCode();

        public ArticleCategoryAppService(
            IEntityRepository<Article> articleEntityRepository,
            IEntityRepository<ArticleCategory> articleCategoryEntityRepository,
            IArticleCategoryRepository articleCategoryRepository,
            IUserRepository userRepository)
        {
            _articleEntityRepository = articleEntityRepository;
            _articleCategoryEntityRepository = articleCategoryEntityRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _userRepository = userRepository;
        }

        #region get
        public async Task<ArticleCategoryResult> FindAsync(IdRequestFilter<int> filter)
        {
            var existing = await _articleCategoryRepository.FindAsync(filter.Id);
            if (existing == null)
            {
                return null;
            }

            if (existing.StatusId == _inactivedStatus && !filter.CanGetInactived)
            {
                return null;
            }
            
            var result = MapEntityToDto(existing);

            var createdByUser = await _userRepository.FindByIdAsync(existing.CreatedById);
            var updatedByUser = await _userRepository.FindByIdAsync(existing.UpdatedById);
            result.CreatedBy = createdByUser.DisplayName;
            result.UpdatedBy = updatedByUser.DisplayName;

            return result;
        }

        public async Task<ArticleCategoryResult> FindByNameAsync(string name)
        {
            var existing = await _articleCategoryRepository.FindByNameAsync(name);
            if (existing == null)
            {
                return null;
            }

            if (existing.StatusId == _inactivedStatus)
            {
                return null;
            }

            return MapEntityToDto(existing); ;
        }

        private ArticleCategoryResult MapEntityToDto(ArticleCategory category)
        {
            return new ArticleCategoryResult
            {
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                CreatedById = category.CreatedById,
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                UpdatedById = category.UpdatedById,
                UpdatedDate = category.UpdatedDate,
                StatusId = category.StatusId,
                ParentCategoryName = category.ParentCategory?.Name,
                ParentCategory = category.ParentCategory != null ? MapEntityToDto(category.ParentCategory) : null
            };
        }

        public async Task<BasePageList<ArticleCategoryResult>> GetAsync(ArticleCategoryFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var categoryQuery = _articleCategoryEntityRepository.Get(x => filter.CanGetInactived || x.StatusId != _inactivedStatus);
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

            var query = categoryQuery.Select(a => new ArticleCategoryResult
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

            await PopulateModifiedUsersAsync(categories);
            var categoryPageList = new BasePageList<ArticleCategoryResult>(categories)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return categoryPageList;
        }

        public async Task PopulateModifiedUsersAsync(IList<ArticleCategoryResult> articleCategories)
        {
            var createdByIds = articleCategories.Select(x => x.CreatedById).ToArray();
            var updatedByIds = articleCategories.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetByIdsAsync(updatedByIds);

            foreach (var category in articleCategories)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }
        }

        public IList<ArticleCategoryResult> SearchParents(IdRequestFilter<int?> idRequestFilter, BaseFilter filter)
        {
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = _articleCategoryEntityRepository.Get(x => !x.ParentId.HasValue && (idRequestFilter.CanGetInactived || x.StatusId != _inactivedStatus))
                .Select(c => new ArticleCategoryResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentId = c.ParentId
                });

            if (idRequestFilter.Id.HasValue)
            {
                query = query.Where(x => x.Id != idRequestFilter.Id);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword));
            }

            if (filter.PageSize > 0)
            {
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var categories = query
                .Select(x => new ArticleCategoryResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId
                })
                .ToList();

            return categories;
        }

        public IList<ArticleCategoryResult> Search(IdRequestFilter<int?> idRequestFilter, BaseFilter filter)
        {
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var queryParents = _articleCategoryEntityRepository.Get(x => !x.ParentId.HasValue && (idRequestFilter.CanGetInactived || x.StatusId != _inactivedStatus));
            var queryChildrens = _articleCategoryEntityRepository.Get(x => x.ParentId.HasValue);

            var query = from parent in queryParents
                        join child in queryChildrens
                        on parent.Id equals child.ParentId into joined
                        from j in joined.DefaultIfEmpty()
                        orderby j.ParentId
                        select new ArticleCategoryResult
                        {
                            Id = j.Id,
                            Name = j.Name,
                            Description = j.Description,
                            ParentId = j.ParentId,
                            ParentCategory = new ArticleCategoryResult()
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

            if (idRequestFilter.Id.HasValue)
            {
                query = query.Where(x => x.Id != idRequestFilter.Id);
            }

            if (filter.PageSize > 0)
            {
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var data = query
                .Select(x => new ArticleCategoryResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    ParentCategory = new ArticleCategoryResult()
                    {
                        Id = x.ParentCategory.Id,
                        Name = x.ParentCategory.Name,
                        Description = x.ParentCategory.Description,
                    }
                })
                .ToList();

            var categories = new List<ArticleCategoryResult>();
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
        public async Task<int> CreateAsync(ArticleCategoryModifyRequest category)
        {
            var newCategory = new ArticleCategory()
            {
                Description = category.Description,
                Name = category.Name,
                ParentId = category.ParentId,
                CreatedById = category.CreatedById,
                UpdatedById = category.UpdatedById,
                UpdatedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                StatusId = ArticleCategoryStatuses.Actived.GetCode()
            };
            return await _articleCategoryRepository.CreateAsync(newCategory);
        }

        public async Task<bool> UpdateAsync(ArticleCategoryModifyRequest category)
        {
            var existing = await _articleCategoryRepository.FindAsync(category.Id);
            existing.Description = category.Description;
            existing.Name = category.Name;
            existing.ParentId = category.ParentId;
            existing.UpdatedById = category.UpdatedById;
            return await _articleCategoryRepository.UpdateAsync(existing);
        }

        public async Task<bool> ActiveAsync(ArticleCategoryModifyRequest request)
        {
            var existing = await _articleCategoryRepository.FindAsync(request.Id);
            existing.StatusId = (int)ArticleCategoryStatuses.Actived;
            existing.UpdatedById = request.UpdatedById;
            return await _articleCategoryRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeactivateAsync(ArticleCategoryModifyRequest request)
        {
            var existing = await _articleCategoryRepository.FindAsync(request.Id);
            existing.StatusId = _inactivedStatus;
            existing.UpdatedById = request.UpdatedById;
            return await _articleCategoryRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var articles = await _articleEntityRepository.GetAsync(x => x.ArticleCategoryId == id);
            if (articles.Any())
            {
                throw new CaminoApplicationException($"Some {nameof(articles)} belong to this category need to be deleted or move to another category");
            }

            return await _articleCategoryRepository.DeleteAsync(id);
        }
        #endregion

        #region category status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (ArticleCategoryStatuses)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<ArticleCategoryStatuses>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion
    }
}
