using Camino.IdentityManager.Models;
using Module.Api.Article.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Article.GraphQL.Resolvers.Contracts
{
    public interface IArticleResolver
    {
        Task<ArticleModel> CreateArticleAsync(ApplicationUser currentUser, ArticleModel criterias);
        Task<ArticleModel> UpdateArticleAsync(ApplicationUser currentUser, ArticleModel criterias);
        Task<ArticlePageListModel> GetUserArticlesAsync(ArticleFilterModel criterias);
        Task<ArticlePageListModel> GetArticlesAsync(ArticleFilterModel criterias);
        Task<ArticleModel> GetArticleAsync(ArticleFilterModel criterias);
        Task<IList<ArticleModel>> GetRelevantArticlesAsync(ArticleFilterModel criterias);
    }
}
