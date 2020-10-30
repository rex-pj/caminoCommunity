using AutoMapper;
using Camino.Data.Contracts;
using Camino.Service.Projections.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using Camino.Service.Business.Articles.Contracts;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Article;

namespace Camino.Service.Business.Articles
{
    public class ArticleCategoryBusiness : IArticleCategoryBusiness
    {
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ArticleCategoryBusiness(IMapper mapper, IRepository<ArticleCategory> articleCategoryRepository,
            IRepository<User> userRepository)
        {
            _mapper = mapper;
            _articleCategoryRepository = articleCategoryRepository;
            _userRepository = userRepository;
        }

        public ArticleCategoryProjection Find(long id)
        {
            var exist = (from child in _articleCategoryRepository.Table
                         join parent in _articleCategoryRepository.Table
                         on child.ParentId equals parent.Id into categories
                         from cate in categories.DefaultIfEmpty()
                         where child.Id == id
                         select new ArticleCategoryProjection
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
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            var createdByUser = _userRepository.FirstOrDefault(x => x.Id == exist.CreatedById);
            var updatedByUser = _userRepository.FirstOrDefault(x => x.Id == exist.UpdatedById);

            var category = _mapper.Map<ArticleCategoryProjection>(exist);
            category.CreatedBy = createdByUser.DisplayName;
            category.UpdatedBy = updatedByUser.DisplayName;

            return category;
        }

        public ArticleCategoryProjection FindByName(string name)
        {
            var exist = _articleCategoryRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var category = _mapper.Map<ArticleCategoryProjection>(exist);

            return category;
        }

        public async Task<BasePageList<ArticleCategoryProjection>> GetAsync(ArticleCategoryFilter filter)
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

            var query = categoryQuery.Select(a => new ArticleCategoryProjection
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

            var createdByIds = categories.Select(x => x.CreatedById).ToArray();
            var updatedByIds = categories.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).ToList();

            foreach (var category in categories)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }


            var result = new BasePageList<ArticleCategoryProjection>(categories);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }

        public List<ArticleCategoryProjection> Get(Expression<Func<ArticleCategory, bool>> filter)
        {
            var categories = _articleCategoryRepository.Get(filter).Select(a => new ArticleCategoryProjection
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            return categories;
        }

        public IList<ArticleCategoryProjection> SearchParents(string search = "", long? currentId = null, int page = 1, int pageSize = 10)
        {
            if (search == null)
            {
                search = string.Empty;
            }

            search = search.ToLower();
            var query = _articleCategoryRepository.Get(x => !x.ParentId.HasValue)
                .Select(c => new ArticleCategoryProjection
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
                .Select(x => new ArticleCategoryProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId
                })
                .ToList();

            return categories;
        }

        public IList<ArticleCategoryProjection> Search(string search = "", long? currentId = null, int page = 1, int pageSize = 10)
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
                        select new ArticleCategoryProjection
                        {
                            Id = j.Id,
                            Name = j.Name,
                            Description = j.Description,
                            ParentId = j.ParentId,
                            ParentCategory = new ArticleCategoryProjection()
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
                .Select(x => new ArticleCategoryProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    ParentCategory = new ArticleCategoryProjection()
                    {
                        Id = x.ParentCategory.Id,
                        Name = x.ParentCategory.Name,
                        Description = x.ParentCategory.Description,
                    }
                })
                .ToList();

            var categories = new List<ArticleCategoryProjection>();
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

        public int Create(ArticleCategoryProjection category)
        {
            var newCategory = _mapper.Map<ArticleCategory>(category);
            newCategory.UpdatedDate = DateTime.UtcNow;
            newCategory.CreatedDate = DateTime.UtcNow;

            var id = _articleCategoryRepository.AddWithInt32Entity(newCategory);
            return id;
        }

        public ArticleCategoryProjection Update(ArticleCategoryProjection category)
        {
            var exist = _articleCategoryRepository.FirstOrDefault(x => x.Id == category.Id);
            exist.Description = category.Description;
            exist.Name = category.Name;
            exist.ParentId = category.ParentId;
            exist.UpdatedById = category.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;

            _articleCategoryRepository.Update(exist);

            category.UpdatedDate = exist.UpdatedDate;
            return category;
        }
    }
}
