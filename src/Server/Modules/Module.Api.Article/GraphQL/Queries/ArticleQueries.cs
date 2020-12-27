using Camino.Framework.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using Module.Api.Article.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Article.GraphQL.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class ArticleQueries : BaseQueries
    {
        public async Task<ArticlePageListModel> GetUserArticlesAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetUserArticlesAsync(criterias);
        }

        public async Task<ArticlePageListModel> GetArticlesAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetArticlesAsync(criterias);
        }

        public async Task<ArticleModel> GetArticleAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetArticleAsync(criterias);
        }

        public async Task<IList<ArticleModel>> GetRelevantArticlesAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetRelevantArticlesAsync(criterias);
        }
    }
}
