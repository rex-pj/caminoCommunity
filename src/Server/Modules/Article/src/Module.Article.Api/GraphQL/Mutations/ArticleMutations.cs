using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Article.Api.GraphQL.Resolvers.Contracts;
using Module.Article.Api.Models;
using System.Threading.Tasks;

namespace Module.Article.Api.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ArticleMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<ArticleModel> GetArticleAsync([Service] IArticleResolver articleResolver, ArticleIdFilterModel criterias)
        {
            return await articleResolver.GetArticleAsync(criterias);
        }
    }
}
