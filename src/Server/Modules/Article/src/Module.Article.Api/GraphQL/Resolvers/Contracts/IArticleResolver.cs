using Module.Article.Api.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Article.Api.GraphQL.Resolvers.Contracts
{
    public interface IArticleResolver
    {
        Task<ArticlePageListModel> GetUserArticlesAsync(ClaimsPrincipal claimsPrincipal, ArticleFilterModel criterias);
        Task<ArticlePageListModel> GetArticlesAsync(ArticleFilterModel criterias);
        Task<ArticleModel> GetArticleAsync(ClaimsPrincipal claimsPrincipal, ArticleIdFilterModel criterias);
        Task<IList<ArticleModel>> GetRelevantArticlesAsync(ArticleFilterModel criterias);
    }
}
