using Camino.Core.Domain.Identities;
using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using Module.Api.Article.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Article.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class ArticleQueries : BaseQueries
    {
        public async Task<ArticlePageListModel> GetUserArticlesAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetUserArticlesAsync(currentUser, criterias);
        }

        public async Task<ArticlePageListModel> GetArticlesAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetArticlesAsync(criterias);
        }

        public async Task<ArticleModel> GetArticleAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetArticleAsync(currentUser, criterias);
        }

        public async Task<IList<ArticleModel>> GetRelevantArticlesAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetRelevantArticlesAsync(criterias);
        }
    }
}
