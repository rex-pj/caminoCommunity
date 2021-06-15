using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.Core.Domain.Identities;
using Camino.Shared.Results.Articles;
using Camino.Shared.Requests.Filters;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using Module.Api.Article.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.IdentityManager;
using Camino.Shared.Requests.Media;
using Camino.Shared.Requests.Articles;
using Camino.Core.Contracts.Services.Articles;

namespace Module.Api.Article.GraphQL.Resolvers
{
    public class ArticleResolver : BaseResolver, IArticleResolver
    {
        private readonly IArticleService _articleService;
        private readonly IUserManager<ApplicationUser> _userManager;

        public ArticleResolver(IArticleService articleService, IUserManager<ApplicationUser> userManager, ISessionContext sessionContext)
            : base(sessionContext)
        {
            _articleService = articleService;
            _userManager = userManager;
        }

        public async Task<ArticleModel> CreateArticleAsync(ApplicationUser currentUser, ArticleModel criterias)
        {
            var article = new ArticleModifyRequest()
            {
                CreatedById = currentUser.Id,
                UpdatedById = currentUser.Id,
                Content = criterias.Content,
                Name = criterias.Name,
                ArticleCategoryId = criterias.ArticleCategoryId
            };

            if (criterias.Picture != null)
            {
                article.Picture = new PictureRequest()
                {
                    Base64Data = criterias.Picture.Base64Data,
                    ContentType = criterias.Picture.ContentType,
                    FileName = criterias.Picture.FileName
                };
            }

            var id = await _articleService.CreateAsync(article);
            criterias.Id = id;
            return criterias;
        }

        public async Task<ArticleModel> UpdateArticleAsync(ApplicationUser currentUser, ArticleModel criterias)
        {
            var exist = await _articleService.FindAsync(criterias.Id);
            if (exist == null)
            {
                throw new Exception("No article found");
            }

            if (currentUser.Id != exist.CreatedById)
            {
                throw new UnauthorizedAccessException();
            }

            var article = new ArticleModifyRequest()
            {
                Id = criterias.Id,
                CreatedById = currentUser.Id,
                UpdatedById = currentUser.Id,
                Content = criterias.Content,
                Name = criterias.Name,
                ArticleCategoryId = criterias.ArticleCategoryId
            };

            if (criterias.Picture != null)
            {
                article.Picture = new PictureRequest()
                {
                    Base64Data = criterias.Picture.Base64Data,
                    ContentType = criterias.Picture.ContentType,
                    FileName = criterias.Picture.FileName,
                    Id = criterias.Picture.PictureId
                };
            }

            await _articleService.UpdateAsync(article);
            return criterias;
        }

        public async Task<ArticlePageListModel> GetUserArticlesAsync(ArticleFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleFilterModel();
            }

            if (string.IsNullOrEmpty(criterias.UserIdentityId))
            {
                return new ArticlePageListModel(new List<ArticleModel>())
                {
                    Filter = criterias
                };
            }

            var userId = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            var filterRequest = new ArticleFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search,
                CreatedById = userId
            };

            try
            {
                var articlePageList = await _articleService.GetAsync(filterRequest);
                var articles = await MapArticlesResultToModelAsync(articlePageList.Collections);

                var articlePage = new ArticlePageListModel(articles)
                {
                    Filter = criterias,
                    TotalPage = articlePageList.TotalPage,
                    TotalResult = articlePageList.TotalResult
                };

                return articlePage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ArticlePageListModel> GetArticlesAsync(ArticleFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleFilterModel();
            }

            var filterRequest = new ArticleFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search
            };

            try
            {
                var articlePageList = await _articleService.GetAsync(filterRequest);
                var articles = await MapArticlesResultToModelAsync(articlePageList.Collections);

                var articlePage = new ArticlePageListModel(articles)
                {
                    Filter = criterias,
                    TotalPage = articlePageList.TotalPage,
                    TotalResult = articlePageList.TotalResult
                };

                return articlePage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ArticleModel> GetArticleAsync(ArticleFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleFilterModel();
            }

            try
            {
                var articleResult = await _articleService.FindDetailAsync(criterias.Id);
                var article = await MapArticleResultToModelAsync(articleResult);
                return article;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<ArticleModel>> GetRelevantArticlesAsync(ArticleFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleFilterModel();
            }

            var filterRequest = new ArticleFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search
            };

            try
            {
                var relevantArticles = await _articleService.GetRelevantsAsync(criterias.Id, filterRequest);
                var products = await MapArticlesResultToModelAsync(relevantArticles);

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteArticleAsync(ApplicationUser currentUser, ArticleFilterModel criterias)
        {
            try
            {
                var exist = await _articleService.FindAsync(criterias.Id);
                if (exist == null || currentUser.Id != exist.CreatedById)
                {
                    return false;
                }

                return await _articleService.SoftDeleteAsync(new ArticleModifyRequest
                {
                    Id = criterias.Id,
                    UpdatedById = currentUser.Id,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ArticleModel> MapArticleResultToModelAsync(ArticleResult articleResult)
        {
            var article = new ArticleModel()
            {
                ArticleCategoryId = articleResult.ArticleCategoryId,
                ArticleCategoryName = articleResult.ArticleCategoryName,
                Content = articleResult.Content,
                Id = articleResult.Id,
                CreatedBy = articleResult.CreatedBy,
                CreatedById = articleResult.CreatedById,
                CreatedDate = articleResult.CreatedDate,
                Description = articleResult.Description,
                Name = articleResult.Name,
                CreatedByPhotoCode = articleResult.CreatedByPhotoCode
            };

            if (articleResult.Picture != null)
            {
                article.Picture = new PictureRequestModel()
                {
                    PictureId = articleResult.Picture.Id
                };
            }

            article.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(article.CreatedById);

            return article;
        }

        private async Task<IList<ArticleModel>> MapArticlesResultToModelAsync(IEnumerable<ArticleResult> articleResults)
        {
            var articles = articleResults.Select(x => new ArticleModel()
            {
                ArticleCategoryId = x.ArticleCategoryId,
                ArticleCategoryName = x.ArticleCategoryName,
                Content = x.Content,
                Id = x.Id,
                CreatedBy = x.CreatedBy,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Name = x.Name,
                Picture = x.Picture != null
                    ? new PictureRequestModel
                    {
                        PictureId = x.Picture.Id,
                        ContentType = x.Picture.ContentType,
                        FileName = x.Picture.FileName
                    }
                    : new PictureRequestModel(),
                CreatedByPhotoCode = x.CreatedByPhotoCode
            }).ToList();

            foreach (var article in articles)
            {
                article.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(article.CreatedById);
                if (!string.IsNullOrEmpty(article.Description) && article.Description.Length >= 150)
                {
                    article.Description = $"{article.Description.Substring(0, 150)}...";
                }
            }

            return articles;
        }
    }
}
