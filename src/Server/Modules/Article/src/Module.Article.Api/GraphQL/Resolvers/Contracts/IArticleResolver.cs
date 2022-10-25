using Module.Article.Api.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Article.Api.GraphQL.Resolvers.Contracts
{
    public interface IArticleResolver
    {
        Task<ArticleIdResultModel> CreateArticleAsync(ClaimsPrincipal claimsPrincipal, CreateArticleModel criterias);
        Task<ArticleIdResultModel> UpdateArticleAsync(ClaimsPrincipal claimsPrincipal, UpdateArticleModel criterias);
        Task<ArticlePageListModel> GetUserArticlesAsync(ClaimsPrincipal claimsPrincipal, ArticleFilterModel criterias);
        Task<ArticlePageListModel> GetArticlesAsync(ArticleFilterModel criterias);
        Task<ArticleModel> GetArticleAsync(ClaimsPrincipal claimsPrincipal, ArticleIdFilterModel criterias);
        Task<IList<ArticleModel>> GetRelevantArticlesAsync(ArticleFilterModel criterias);
        Task<bool> DeleteArticleAsync(ClaimsPrincipal claimsPrincipal, ArticleIdFilterModel criterias);
    }
}
