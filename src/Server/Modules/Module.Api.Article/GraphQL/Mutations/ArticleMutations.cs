using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.IdentityManager.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using Module.Api.Article.Models;
using System.Threading.Tasks;

namespace Module.Api.Article.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class ArticleMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<ArticleModel> CreateArticleAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IArticleResolver articleResolver, ArticleModel criterias)
        {
            return await articleResolver.CreateArticleAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<ArticleModel> UpdateArticleAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IArticleResolver articleResolver, ArticleModel criterias)
        {
            return await articleResolver.UpdateArticleAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<bool> DeleteArticleAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.DeleteArticleAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<ArticleModel> GetArticleAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetArticleAsync(criterias);
        }
    }
}
