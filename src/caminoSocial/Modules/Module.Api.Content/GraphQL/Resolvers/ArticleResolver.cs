using Camino.Framework.GraphQL.Resolvers;
using Camino.IdentityManager.Contracts.Core;
using Camino.Service.Business.Articles.Contracts;
using Camino.Service.Projections.Content;
using Module.Api.Content.GraphQL.Resolvers.Contracts;
using Module.Api.Content.Models;
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
    }
}
