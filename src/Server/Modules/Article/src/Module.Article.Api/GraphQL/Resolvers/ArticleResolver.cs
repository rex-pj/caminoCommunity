using Camino.Infrastructure.GraphQL.Resolvers;
using Module.Article.Api.GraphQL.Resolvers.Contracts;
using Module.Article.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Camino.Application.Contracts.AppServices.Articles;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Core;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Articles.Dtos.Dtos;
using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Article.Api.GraphQL.Resolvers
{
    public class ArticleResolver : BaseResolver, IArticleResolver
    {
        private readonly IArticleAppService _articleAppService;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly PagerOptions _pagerOptions;

        public ArticleResolver(IArticleAppService articleAppService, IUserManager<ApplicationUser> userManager,
            IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _articleAppService = articleAppService;
            _userManager = userManager;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<ArticlePageListModel> GetUserArticlesAsync(ClaimsPrincipal claimsPrincipal, ArticleFilterModel criterias)
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

            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var userId = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            var filterRequest = new ArticleFilter
            {
                Page = criterias.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = criterias.Search,
                CreatedById = userId,
                CanGetInactived = currentUserId == userId
            };

            try
            {
                var articlePageList = await _articleAppService.GetAsync(filterRequest);
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

            var filterRequest = new ArticleFilter
            {
                Page = criterias.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = criterias.Search
            };

            try
            {
                var articlePageList = await _articleAppService.GetAsync(filterRequest);
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

        public async Task<ArticleModel> GetArticleAsync(ArticleIdFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleIdFilterModel();
            }

            if (criterias.Id <= 0)
            {
                return new ArticleModel();
            }

            var articleResult = await _articleAppService.FindDetailAsync(new IdRequestFilter<long>
            {
                Id = criterias.Id,
                CanGetInactived = true
            });

            var article = await MapArticleResultToModelAsync(articleResult);
            return article;
        }

        public async Task<IList<ArticleModel>> GetRelevantArticlesAsync(ArticleFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleFilterModel();
            }

            if (criterias.Id <= 0)
            {
                return new List<ArticleModel>();
            }

            var filterRequest = new ArticleFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize.HasValue && criterias.PageSize < _pagerOptions.PageSize ? criterias.PageSize.Value : _pagerOptions.PageSize,
                Keyword = criterias.Search
            };

            try
            {
                var relevantArticles = await _articleAppService.GetRelevantsAsync(criterias.Id.GetValueOrDefault(), filterRequest);
                var products = await MapArticlesResultToModelAsync(relevantArticles);

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ArticleModel> MapArticleResultToModelAsync(ArticleResult articleResult)
        {
            var article = new ArticleModel
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
                CreatedByPhotoId = articleResult.CreatedByPhotoId
            };

            if (articleResult.Picture != null)
            {
                article.Picture = new PictureResultModel()
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
                    ? new PictureResultModel
                    {
                        PictureId = x.Picture.Id,
                        ContentType = x.Picture.ContentType,
                        FileName = x.Picture.FileName
                    }
                    : new PictureResultModel(),
                CreatedByPhotoId = x.CreatedByPhotoId
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
