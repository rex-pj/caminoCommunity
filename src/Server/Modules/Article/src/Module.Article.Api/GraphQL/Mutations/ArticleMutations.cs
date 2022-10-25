using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Article.Api.GraphQL.Resolvers.Contracts;
using Module.Article.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Article.Api.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ArticleMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<ArticleIdResultModel> CreateArticleAsync(ClaimsPrincipal claimsPrincipal, [Service] IArticleResolver articleResolver, CreateArticleModel criterias)
        {
            return await articleResolver.CreateArticleAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<ArticleIdResultModel> UpdateArticleAsync(ClaimsPrincipal claimsPrincipal, [Service] IArticleResolver articleResolver, UpdateArticleModel criterias)
        {
            return await articleResolver.UpdateArticleAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<bool> DeleteArticleAsync(ClaimsPrincipal claimsPrincipal, [Service] IArticleResolver articleResolver, ArticleIdFilterModel criterias)
        {
            return await articleResolver.DeleteArticleAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<ArticleModel> GetArticleAsync(ClaimsPrincipal claimsPrincipal, [Service] IArticleResolver articleResolver, ArticleIdFilterModel criterias)
        {
            return await articleResolver.GetArticleAsync(claimsPrincipal, criterias);
        }
    }
}
