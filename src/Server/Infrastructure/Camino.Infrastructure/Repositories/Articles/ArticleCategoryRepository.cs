﻿using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Articles;
using Camino.Core.Domain.Articles;
using Camino.Shared.Requests.Articles;

namespace Camino.Service.Repository.Articles
{
    public class ArticleCategoryRepository : IArticleCategoryRepository
    {
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;

        public ArticleCategoryRepository(IRepository<ArticleCategory> articleCategoryRepository)
        {
            _articleCategoryRepository = articleCategoryRepository;
        }

        public async Task<ArticleCategoryResult> FindAsync(long id)
        {
            var category = await (from child in _articleCategoryRepository.Table
                            join parent in _articleCategoryRepository.Table
                            on child.ParentId equals parent.Id into categories
                            from cate in categories.DefaultIfEmpty()
                            where child.Id == id
                            select new ArticleCategoryResult
                            {
                                Description = child.Description,
                                CreatedDate = child.CreatedDate,
                                CreatedById = child.CreatedById,
                                Id = child.Id,
                                Name = child.Name,
                                ParentId = child.ParentId,
                                UpdatedById = child.UpdatedById,
                                UpdatedDate = child.UpdatedDate,
                                ParentCategoryName = cate != null ? cate.Name : null
                            }).FirstOrDefaultAsync();

            return category;
        }

        public async Task<ArticleCategoryResult> FindByNameAsync(string name)
        {
            var category = await _articleCategoryRepository.Get(x => x.Name == name)
                .Select(x => new ArticleCategoryResult
                {
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    UpdatedById = x.UpdatedById,
                    UpdatedDate = x.UpdatedDate

                })
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<BasePageList<ArticleCategoryResult>> GetAsync(ArticleCategoryFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var categoryQuery = _articleCategoryRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                categoryQuery = categoryQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
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
                UpdatedDate = a.UpdatedDate
            });

            var filteredNumber = query.Select(x => x.Id).Count();

            var categories = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ArticleCategoryResult>(categories)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public IList<ArticleCategoryResult> SearchParents(string search = "", long? currentId = null, int page = 1, int pageSize = 10)
        {
            if (search == null)
            {
                search = string.Empty;
            }

            search = search.ToLower();
            var query = _articleCategoryRepository.Get(x => !x.ParentId.HasValue)
                .Select(c => new ArticleCategoryResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentId = c.ParentId
                });

            if (currentId.HasValue)
            {
                query = query.Where(x => x.Id != currentId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search) || x.Description.ToLower().Contains(search));
            }

            if (pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
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

        public IList<ArticleCategoryResult> Search(string search = "", long? currentId = null, int page = 1, int pageSize = 10)
        {
            if (search == null)
            {
                search = string.Empty;
            }

            search = search.ToLower();
            var queryParents = _articleCategoryRepository.Get(x => !x.ParentId.HasValue);
            var queryChildrens = _articleCategoryRepository.Get(x => x.ParentId.HasValue);

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

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search)
                        || x.Description.ToLower().Contains(search)
                        || x.ParentCategory.Name.ToLower().Contains(search)
                        || x.ParentCategory.Description.ToLower().Contains(search));
            }

            if (currentId.HasValue)
            {
                query = query.Where(x => x.Id != currentId);
            }

            if (pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
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
                IsPublished = true
            };

            var id = await _articleCategoryRepository.AddWithInt32EntityAsync(newCategory);
            return id;
        }

        public async Task<bool> UpdateAsync(ArticleCategoryModifyRequest category)
        {
            var exist = await _articleCategoryRepository.Get(x => x.Id == category.Id)
                .Set(x => x.Description, category.Description)
                .Set(x => x.Name, category.Name)
                .Set(x => x.ParentId, category.ParentId)
                .Set(x => x.UpdatedById, category.UpdatedById)
                .Set(x => x.UpdatedDate, DateTime.UtcNow)
                .UpdateAsync();

            return true;
        }
    }
}
