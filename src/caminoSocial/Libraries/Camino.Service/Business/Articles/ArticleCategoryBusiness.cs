using AutoMapper;
using Camino.Data.Contracts;
using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using Camino.Service.Business.Articles.Contracts;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Data.Page;

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

        public ArticleCategoryResult Find(int id)
        {
            var exist = (from child in _articleCategoryRepository.Table
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
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            var createdByUser = _userRepository.FirstOrDefault(x => x.Id == exist.CreatedById);
            var updatedByUser = _userRepository.FirstOrDefault(x => x.Id == exist.UpdatedById);

            var category = _mapper.Map<ArticleCategoryResult>(exist);
            category.CreatedBy = createdByUser.DisplayName;
            category.UpdatedBy = updatedByUser.DisplayName;

            return category;
        }

        public ArticleCategoryResult FindByName(string name)
        {
            var exist = _articleCategoryRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var category = _mapper.Map<ArticleCategoryResult>(exist);

            return category;
        }

        public async Task<PageList<ArticleCategoryResult>> GetAsync(ArticleCategoryFilter filter)
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


            var result = new PageList<ArticleCategoryResult>(categories);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }

        public List<ArticleCategoryResult> Get(Expression<Func<ArticleCategory, bool>> filter)
        {
            var categories = _articleCategoryRepository.Get(filter).Select(a => new ArticleCategoryResult
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            return categories;
        }

        public List<ArticleCategoryResult> Get()
        {
            var parentCategories = _articleCategoryRepository.Get(x => !x.ParentId.HasValue)
                .Select(a => new ArticleCategoryResult
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description
                }).ToList();

            var childCategories = _articleCategoryRepository.Get(x => x.ParentId.HasValue)
                .OrderBy(x => x.ParentId).Select(a => new ArticleCategoryResult
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ParentId = a.ParentId
                }).ToList();

            var categories = new List<ArticleCategoryResult>();
            foreach(var category in parentCategories)
            {
                categories.Add(category);
                var subCategories = childCategories.Where(x => x.ParentId == category.Id);
                if(subCategories != null)
                {
                    categories.AddRange(subCategories);
                }
            }

            return categories;
        }

        public int Add(ArticleCategoryResult category)
        {
            var newCategory = _mapper.Map<ArticleCategory>(category);
            newCategory.UpdatedDate = DateTime.UtcNow;
            newCategory.CreatedDate = DateTime.UtcNow;

            var id = _articleCategoryRepository.AddWithInt32Entity(newCategory);
            return id;
        }

        public ArticleCategoryResult Update(ArticleCategoryResult category)
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
