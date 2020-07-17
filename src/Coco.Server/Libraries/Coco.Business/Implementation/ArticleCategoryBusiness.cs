using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Core.Dtos.Content;
using Coco.Core.Entities.Content;
using Coco.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Coco.Business.Implementation
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

        public ArticleCategoryDto Find(int id)
        {
            var exist = (from child in _articleCategoryRepository.Table
                        join parent in _articleCategoryRepository.Table
                        on child.ParentId equals parent.Id into categories
                        from cate in categories.DefaultIfEmpty()
                        where cate.Id == id
                        select new ArticleCategoryDto
                        {
                            Description = child.Description,
                            CreatedDate = child.CreatedDate,
                            CreatedById = child.CreatedById,
                            Id = child.Id,
                            Name = child.Name,
                            ParentId = child.ParentId,
                            UpdatedById = child.UpdatedById,
                            UpdatedDate = child.UpdatedDate,
                            ParentCategoryName = cate.Name
                        }).FirstOrDefault();

            var createdByUser = _userRepository.FirstOrDefault(x => x.Id == exist.CreatedById);
            var updatedByUser = _userRepository.FirstOrDefault(x => x.Id == exist.UpdatedById);

            var category = _mapper.Map<ArticleCategoryDto>(exist);
            category.CreatedBy = createdByUser.DisplayName;
            category.UpdatedBy = updatedByUser.DisplayName;

            return category;
        }

        public ArticleCategoryDto FindByName(string name)
        {
            var exist = _articleCategoryRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var category = _mapper.Map<ArticleCategoryDto>(exist);
            
            return category;
        }

        public List<ArticleCategoryDto> GetFull()
        {
            var categories = _articleCategoryRepository.Get().Select(a => new ArticleCategoryDto
            {
                CreatedById = a.CreatedById,
                CreatedDate = a.CreatedDate,
                Description = a.Description,
                Id = a.Id,
                Name = a.Name,
                ParentId = a.ParentId,
                UpdatedById = a.UpdatedById,
                UpdatedDate = a.UpdatedDate
            }).ToList();

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

            return categories;
        }

        public List<ArticleCategoryDto> Get(Expression<Func<ArticleCategory, bool>> filter)
        {
            var categories = _articleCategoryRepository.Get(filter).Select(a => new ArticleCategoryDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            return categories;
        }

        public int Add(ArticleCategoryDto category)
        {
            var newCategory = _mapper.Map<ArticleCategory>(category);
            newCategory.UpdatedDate = DateTime.UtcNow;
            newCategory.CreatedDate = DateTime.UtcNow;

            var id = _articleCategoryRepository.Add(newCategory);
            return (int)id;
        }

        public ArticleCategoryDto Update(ArticleCategoryDto category)
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
