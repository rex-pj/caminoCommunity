using Module.Api.Article.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Article.GraphQL.Resolvers.Contracts
{
    public interface IArticleResolver
    {
        Task<ArticleModel> CreateArticleAsync(ClaimsPrincipal claimsPrincipal, ArticleModel criterias);
        Task<ArticleModel> UpdateArticleAsync(ClaimsPrincipal claimsPrincipal, ArticleModel criterias);
        Task<ArticlePageListModel> GetUserArticlesAsync(ClaimsPrincipal claimsPrincipal, ArticleFilterModel criterias);
        Task<ArticlePageListModel> GetArticlesAsync(ArticleFilterModel criterias);
        Task<ArticleModel> GetArticleAsync(ClaimsPrincipal claimsPrincipal, ArticleIdFilterModel criterias);
        Task<IList<ArticleModel>> GetRelevantArticlesAsync(ArticleFilterModel criterias);
        Task<bool> DeleteArticleAsync(ClaimsPrincipal claimsPrincipal, ArticleIdFilterModel criterias);
    }
}
