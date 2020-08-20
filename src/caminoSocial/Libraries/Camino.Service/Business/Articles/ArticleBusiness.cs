using AutoMapper;
using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using Camino.Data.Contracts;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Articles.Contracts;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Data.PageList;

namespace Camino.Service.Business.Articles
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

        public ArticleProjection Find(int id)
        {
            var exist = (from article in _articleRepository.Table
                         join category in _articleCategoryRepository.Table
                         on article.ArticleCategoryId equals category.Id
                         where article.Id == id
                         select new ArticleProjection
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

            var articleResult = _mapper.Map<ArticleProjection>(exist);
            articleResult.CreatedBy = createdByUser.DisplayName;
            articleResult.UpdatedBy = updatedByUser.DisplayName;

            return articleResult;
        }

        public ArticleProjection FindByName(string name)
        {
            var exist = _articleRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var article = _mapper.Map<ArticleProjection>(exist);

            return article;
        }

        public async Task<BasePageList<ArticleProjection>> GetAsync(ArticleFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var articleQuery = _articleRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                articleQuery = articleQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            var content = filter.Content != null ? filter.Content.ToLower() : "";
            if (!string.IsNullOrEmpty(content))
            {
                articleQuery = articleQuery.Where(user => user.Content.ToLower().Contains(content));
            }

            if (filter.CreatedById.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var query = articleQuery.Select(a => new ArticleProjection
            {
                CreatedById = a.CreatedById,
                CreatedDate = a.CreatedDate,
                Description = a.Description,
                Id = a.Id,
                Name = a.Name,
                UpdatedById = a.UpdatedById,
                UpdatedDate = a.UpdatedDate
            });

            var filteredNumber = query.Select(x => x.Id).Count();

            var articles  = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

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


            var result = new BasePageList<ArticleProjection>(articles);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }

        public int Add(ArticleProjection category)
        {
            var newArticle = _mapper.Map<Article>(category);
            newArticle.UpdatedDate = DateTime.UtcNow;
            newArticle.CreatedDate = DateTime.UtcNow;

            var id = _articleRepository.AddWithInt32Entity(newArticle);
            return (int)id;
        }

        public ArticleProjection Update(ArticleProjection article)
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
