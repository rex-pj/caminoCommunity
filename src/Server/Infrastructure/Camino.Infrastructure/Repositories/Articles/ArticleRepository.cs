using Camino.Shared.Requests.Filters;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using System.Collections.Generic;
using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Articles;
using Camino.Core.Domain.Articles;
using Camino.Shared.Requests.Articles;
using Camino.Shared.Enums;

namespace Camino.Service.Repository.Articles
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;

        public ArticleRepository(IRepository<Article> articleRepository, IRepository<ArticleCategory> articleCategoryRepository)
        {
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
        }

        public async Task<ArticleResult> FindAsync(long id)
        {
            var exist = await (from article in _articleRepository.Get(x => x.Id == id && !x.IsDeleted)
                               join category in _articleCategoryRepository.Table
                               on article.ArticleCategoryId equals category.Id
                               select new ArticleResult
                               {
                                   CreatedDate = article.CreatedDate,
                                   CreatedById = article.CreatedById,
                                   Id = article.Id,
                                   Name = article.Name,
                                   UpdatedById = article.UpdatedById,
                                   UpdatedDate = article.UpdatedDate,
                                   ArticleCategoryName = category.Name,
                                   ArticleCategoryId = article.ArticleCategoryId
                               }).FirstOrDefaultAsync();
            return exist;
        }

        public async Task<ArticleResult> FindDetailAsync(long id)
        {
            var exist = await (from article in _articleRepository.Get(x => x.Id == id && !x.IsDeleted)
                               join category in _articleCategoryRepository.Table
                               on article.ArticleCategoryId equals category.Id
                               where article.Id == id && !article.IsDeleted
                               select new ArticleResult
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
                                   Content = article.Content
                               }).FirstOrDefaultAsync();

            return exist;
        }

        public ArticleResult FindByName(string name)
        {
            var article = _articleRepository.Get(x => x.Name == name && !x.IsDeleted)
                .Select(x => new ArticleResult()
                {
                    ArticleCategoryId = x.ArticleCategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    Content = x.Content,
                    UpdatedById = x.UpdatedById,
                    UpdatedDate = x.UpdatedDate,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id
                })
                .FirstOrDefault();

            return article;
        }

        public async Task<BasePageList<ArticleResult>> GetAsync(ArticleFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var articleQuery = _articleRepository.Get(x => filter.IsGettingDeleted || !x.IsDeleted);
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

            if (filter.CategoryId.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.ArticleCategoryId == filter.CategoryId);
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

            var filteredNumber = articleQuery.Select(x => x.Id).Count();

            var query = from ar in articleQuery
                        select new ArticleResult
                        {
                            Id = ar.Id,
                            Name = ar.Name,
                            CreatedById = ar.CreatedById,
                            CreatedDate = ar.CreatedDate,
                            Description = ar.Description,
                            UpdatedById = ar.UpdatedById,
                            UpdatedDate = ar.UpdatedDate,
                            Content = ar.Content,
                            StatusId = ar.StatusId,
                            IsDeleted = ar.IsDeleted
                        };

            var articles = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ArticleResult>(articles)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<int> CreateAsync(ArticleModifyRequest request)
        {
            var newArticle = new Article()
            {
                ArticleCategoryId = request.ArticleCategoryId,
                Content = request.Content,
                CreatedById = request.CreatedById,
                UpdatedById = request.UpdatedById,
                CreatedDate = request.CreatedDate,
                UpdatedDate = request.UpdatedDate,
                Description = request.Description,
                Name = request.Name,
                IsPublished = true
            };

            return await _articleRepository.AddWithInt32EntityAsync(newArticle);
        }

        public async Task<bool> UpdateAsync(ArticleModifyRequest request)
        {
            var exist = _articleRepository.FirstOrDefault(x => x.Id == request.Id);
            exist.Name = request.Name;
            exist.ArticleCategoryId = request.ArticleCategoryId;
            exist.UpdatedById = request.UpdatedById;
            exist.UpdatedDate = request.UpdatedDate;
            exist.Content = request.Content;

            await _articleRepository.UpdateAsync(exist);
            return true;
        }

        public async Task<IList<ArticleResult>> GetRelevantsAsync(long id, ArticleFilter filter)
        {
            var exist = (from ar in _articleRepository.Get(x => x.Id == id && !x.IsDeleted)
                         select new ArticleResult
                         {
                             Id = ar.Id,
                             CreatedById = ar.CreatedById,
                             UpdatedById = ar.UpdatedById,
                             ArticleCategoryId = ar.ArticleCategoryId
                         }).FirstOrDefault();

            var relevantArticleQuery = (from ar in _articleRepository.Get(x => x.Id != exist.Id && !x.IsDeleted)
                                        where ar.CreatedById == exist.CreatedById
                                        || ar.ArticleCategoryId == exist.ArticleCategoryId
                                        || ar.UpdatedById == exist.UpdatedById
                                        select new ArticleResult
                                        {
                                            Id = ar.Id,
                                            Name = ar.Name,
                                            CreatedById = ar.CreatedById,
                                            CreatedDate = ar.CreatedDate,
                                            Description = ar.Description,
                                            UpdatedById = ar.UpdatedById,
                                            UpdatedDate = ar.UpdatedDate,
                                            Content = ar.Content,
                                        });

            var relevantArticles = await relevantArticleQuery
                .OrderByDescending(x => x.CreatedDate).Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            return relevantArticles;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _articleRepository.Get(x => x.Id == id)
                .DeleteAsync();

            return true;
        }

        public async Task<bool> SoftDeleteAsync(long id)
        {
            await _articleRepository.Get(x => x.Id == id)
                .Set(x => x.IsDeleted, true)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(long id)
        {
            await _articleRepository.Get(x => x.Id == id)
                .Set(x => x.StatusId, (int)ArticleStatus.Inactived)
                .UpdateAsync();

            return true;
        }
    }
}
