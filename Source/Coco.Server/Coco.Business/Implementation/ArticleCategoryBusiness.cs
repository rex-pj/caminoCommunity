using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.DAL;
using Coco.Entities.Domain.Content;
using Coco.Entities.Dtos.Content;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class ArticleCategoryBusiness : IArticleCategoryBusiness
    {
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ContentDbContext _contentDbContext;

        public ArticleCategoryBusiness(IMapper mapper, IRepository<ArticleCategory> articleCategoryRepository, ContentDbContext contentDbContext)
        {
            _mapper = mapper;
            _articleCategoryRepository = articleCategoryRepository;
            _contentDbContext = contentDbContext;
        }

        public ArticleCategoryDto Find(int id)
        {
            var exist = _articleCategoryRepository.Find(id);
            return _mapper.Map<ArticleCategoryDto>(exist);
        }

        public async Task<List<ArticleCategoryDto>> GetAsync()
        {
            var categories = await _articleCategoryRepository.GetAsync();
            return _mapper.Map<List<ArticleCategoryDto>>(categories);
        }

        public long Add(ArticleCategoryDto category)
        {
            var newCategory = _mapper.Map<ArticleCategory>(category);
            newCategory.UpdatedDate = DateTime.UtcNow;
            newCategory.CreatedDate = DateTime.UtcNow;

            _articleCategoryRepository.Add(newCategory);
            return _contentDbContext.SaveChanges();
        }

        public ArticleCategoryDto Update(ArticleCategoryDto category)
        {
            var exist = _articleCategoryRepository.Find(category.Id);
            exist.Description = category.Description;
            exist.Name = category.Name;
            exist.ParentCategoryId = category.ParentCategoryId;
            exist.UpdatedById = category.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;

            _articleCategoryRepository.Update(exist);
            _contentDbContext.SaveChanges();

            category.UpdatedDate = exist.UpdatedDate;
            return category;
        }
    }
}
