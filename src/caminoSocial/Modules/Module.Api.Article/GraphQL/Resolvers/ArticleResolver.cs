using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Contracts.Core;
using Camino.IdentityManager.Models;
using Camino.Service.Business.Articles.Contracts;
using Camino.Service.Projections.Content;
using Camino.Service.Projections.Filters;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using Module.Api.Article.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Api.Article.GraphQL.Resolvers
{
    public class ArticleResolver : BaseResolver, IArticleResolver
    {
        private readonly IArticleBusiness _articleBusiness;
        private readonly IUserManager<ApplicationUser> _userManager;

        public ArticleResolver(SessionState sessionState, IArticleBusiness articleBusiness, IUserManager<ApplicationUser> userManager)
            : base(sessionState)
        {
            _articleBusiness = articleBusiness;
            _userManager = userManager;
        }

        public async Task<ArticleModel> CreateArticleAsync(ArticleModel criterias)
        {
            var article = new ArticleProjection()
            {
                CreatedById = CurrentUser.Id,
                UpdatedById = CurrentUser.Id,
                Content = criterias.Content,
                Name = criterias.Name,
                Thumbnail = criterias.Thumbnail,
                ThumbnailFileName = criterias.ThumbnailFileName,
                ThumbnailFileType = criterias.ThumbnailFileType,
                ArticleCategoryId = criterias.ArticleCategoryId
            };

            var id = await _articleBusiness.CreateAsync(article);
            criterias.Id = id;
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
                var articlePageList = await _articleBusiness.GetAsync(filterRequest);
                var articles = articlePageList.Collections.Select(x => new ArticleModel()
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
                    ThumbnailId = x.ThumbnailId,
                    ThumbnailFileType = x.ThumbnailFileType,
                    ThumbnailFileName = x.ThumbnailFileName,
                    CreatedByPhotoCode = x.CreatedByPhotoCode
                }).ToList();

                foreach (var article in articles)
                {
                    article.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(article.CreatedById);
                }

                var articlePage = new ArticlePageListModel(articles)
                {
                    Filter = criterias,
                    TotalPage = articlePageList.TotalPage,
                    TotalResult = articlePageList.TotalResult
                };

                return articlePage;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
