using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Models;
using Camino.Service.Business.Articles.Contracts;
using Camino.Service.Projections.Article;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Media;
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

        public ArticleResolver(IArticleBusiness articleBusiness, IUserManager<ApplicationUser> userManager, ISessionContext sessionContext)
            : base(sessionContext)
        {
            _articleBusiness = articleBusiness;
            _userManager = userManager;
        }

        public async Task<ArticleModel> CreateArticleAsync(ApplicationUser currentUser, ArticleModel criterias)
        {
            var article = new ArticleProjection()
            {
                CreatedById = currentUser.Id,
                UpdatedById = currentUser.Id,
                Content = criterias.Content,
                Name = criterias.Name,
                ArticleCategoryId = criterias.ArticleCategoryId
            };

            if (criterias.Thumbnail != null)
            {
                article.Thumbnail = new PictureRequestProjection()
                {
                    Base64Data = criterias.Thumbnail.Base64Data,
                    ContentType = criterias.Thumbnail.ContentType,
                    FileName = criterias.Thumbnail.FileName
                };
            }

            var id = await _articleBusiness.CreateAsync(article);
            criterias.Id = id;
            return criterias;
        }

        public async Task<ArticleModel> UpdateArticleAsync(ApplicationUser currentUser, ArticleModel criterias)
        {
            var exist = await _articleBusiness.FindAsync(criterias.Id);
            if (exist == null)
            {
                throw new Exception("No article found");
            }

            if (currentUser.Id != exist.CreatedById)
            {
                throw new UnauthorizedAccessException();
            }

            var article = new ArticleProjection()
            {
                Id = criterias.Id,
                CreatedById = currentUser.Id,
                UpdatedById = currentUser.Id,
                Content = criterias.Content,
                Name = criterias.Name,
                ArticleCategoryId = criterias.ArticleCategoryId
            };

            if (criterias.Thumbnail != null)
            {
                article.Thumbnail = new PictureRequestProjection()
                {
                    Base64Data = criterias.Thumbnail.Base64Data,
                    ContentType = criterias.Thumbnail.ContentType,
                    FileName = criterias.Thumbnail.FileName,
                    Id = criterias.Thumbnail.PictureId
                };
            }

            var updated = await _articleBusiness.UpdateAsync(article);
            criterias.Id = updated.Id;
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
                var articles = await MapArticlesProjectionToModelAsync(articlePageList.Collections);

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
                var articlePageList = await _articleBusiness.GetAsync(filterRequest);
                var articles = await MapArticlesProjectionToModelAsync(articlePageList.Collections);

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

        public async Task<ArticleModel> GetArticleAsync(ArticleFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleFilterModel();
            }

            try
            {
                var productProjection = await _articleBusiness.FindDetailAsync(criterias.Id);
                var product = await MapArticleProjectionToModelAsync(productProjection);
                return product;
            }
            catch (Exception e)
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
                var relevantArticles = await _articleBusiness.GetRelevantsAsync(criterias.Id, filterRequest);
                var products = await MapArticlesProjectionToModelAsync(relevantArticles);

                return products;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<ArticleModel> MapArticleProjectionToModelAsync(ArticleProjection articleProjection)
        {
            var article = new ArticleModel()
            {
                ArticleCategoryId = articleProjection.ArticleCategoryId,
                ArticleCategoryName = articleProjection.ArticleCategoryName,
                Content = articleProjection.Content,
                Id = articleProjection.Id,
                CreatedBy = articleProjection.CreatedBy,
                CreatedById = articleProjection.CreatedById,
                CreatedDate = articleProjection.CreatedDate,
                Description = articleProjection.Description,
                Name = articleProjection.Name,
                CreatedByPhotoCode = articleProjection.CreatedByPhotoCode
            };

            if (articleProjection.Thumbnail != null)
            {
                article.Thumbnail = new PictureRequestModel()
                {
                    PictureId = articleProjection.Thumbnail.Id
                };
            }

            article.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(article.CreatedById);

            return article;
        }

        private async Task<IList<ArticleModel>> MapArticlesProjectionToModelAsync(IEnumerable<ArticleProjection> articleProjections)
        {
            var articles = articleProjections.Select(x => new ArticleModel()
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
                Thumbnail = x.Thumbnail != null
                    ? new PictureRequestModel
                    {
                        PictureId = x.Thumbnail.Id,
                        ContentType = x.Thumbnail.ContentType,
                        FileName = x.Thumbnail.FileName
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
