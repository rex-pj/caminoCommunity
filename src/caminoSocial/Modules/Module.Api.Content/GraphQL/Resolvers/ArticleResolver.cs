using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.IdentityManager.Contracts.Core;
using Camino.Service.Business.Articles.Contracts;
using Camino.Service.Projections.Content;
using Camino.Service.Projections.Filters;
using Module.Api.Content.GraphQL.Resolvers.Contracts;
using Module.Api.Content.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Api.Content.GraphQL.Resolvers
{
    public class ArticleResolver : BaseResolver, IArticleResolver
    {
        private readonly IArticleBusiness _articleBusiness;

        public ArticleResolver(SessionState sessionState, IArticleBusiness articleBusiness)
            : base(sessionState)
        {
            _articleBusiness = articleBusiness;
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

        public async Task<PageListModel<ArticleModel>> GetArticlesAsync(ArticleFilterModel criterias)
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

            var articlePageList = await _articleBusiness.GetAsync(filterRequest);
            var articles = articlePageList.Collections.Select(x => new ArticleModel() { 
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
                ThumbnailFileName = x.ThumbnailFileName
            });

            var articlePage = new PageListModel<ArticleModel>(articles)
            {
                Filter = criterias,
                TotalPage = articlePageList.TotalPage,
                TotalResult = articlePageList.TotalResult
            };

            return articlePage;
        }
    }
}
