using AutoMapper;
using Camino.Business.Contracts;
using Camino.Business.Dtos.Content;
using Camino.Data.Contracts;
using Camino.Data.Entities.Content;
using Camino.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Business.Implementation
{
    public class ArticleBusiness : IArticleBusiness
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ArticleBusiness(IMapper mapper, IRepository<Article> articleRepository,
            IRepository<ArticleCategory> articleCategoryRepository, IRepository<User> userRepository)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _userRepository = userRepository;
        }

        public ArticleDto Find(int id)
        {
            var exist = (from article in _articleRepository.Table
                         join category in _articleCategoryRepository.Table
                         on article.ArticleCategoryId equals category.Id
                         where article.Id == id
                         select new ArticleDto
                         {
                             Description = article.Description,
                             CreatedDate = article.CreatedDate,
                             CreatedById = article.CreatedById,
                             Id = article.Id,
                             Name = article.Name,
                             UpdatedById = article.UpdatedById,
                             UpdatedDate = article.UpdatedDate,
                             ArticleCategoryName = category.Name,
                             ArticleCategoryId = article.ArticleCategoryId,
                             Content = article.Content,
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            var createdByUser = _userRepository.FirstOrDefault(x => x.Id == exist.CreatedById);
            var updatedByUser = _userRepository.FirstOrDefault(x => x.Id == exist.UpdatedById);

            var articleResult = _mapper.Map<ArticleDto>(exist);
            articleResult.CreatedBy = createdByUser.DisplayName;
            articleResult.UpdatedBy = updatedByUser.DisplayName;

            return articleResult;
        }

        public ArticleDto FindByName(string name)
        {
            var exist = _articleRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var article = _mapper.Map<ArticleDto>(exist);

            return article;
        }

        public List<ArticleDto> GetFull()
        {
            var articles = _articleRepository.Get().Select(a => new ArticleDto
            {
                CreatedById = a.CreatedById,
                CreatedDate = a.CreatedDate,
                Description = a.Description,
                Id = a.Id,
                Name = a.Name,
                ArticleCategoryId = a.ArticleCategoryId,
                UpdatedById = a.UpdatedById,
                UpdatedDate = a.UpdatedDate
            }).ToList();

            var createdByIds = articles.Select(x => x.CreatedById).ToArray();
            var updatedByIds = articles.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).ToList();

            foreach (var category in articles)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }

            return articles;
        }

        public int Add(ArticleDto category)
        {
            var newArticle = _mapper.Map<Article>(category);
            newArticle.UpdatedDate = DateTime.UtcNow;
            newArticle.CreatedDate = DateTime.UtcNow;

            var id = _articleRepository.Add(newArticle);
            return (int)id;
        }

        public ArticleDto Update(ArticleDto article)
        {
            var exist = _articleRepository.FirstOrDefault(x => x.Id == article.Id);
            exist.Description = article.Description;
            exist.Name = article.Name;
            exist.ArticleCategoryId = article.ArticleCategoryId;
            exist.UpdatedById = article.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;
            exist.Content = article.Content;

            _articleRepository.Update(exist);

            article.UpdatedDate = exist.UpdatedDate;
            return article;
        }
    }
}
