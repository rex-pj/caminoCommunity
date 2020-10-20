using Module.Api.Article.Models;
using System.Threading.Tasks;

namespace Module.Api.Article.GraphQL.Resolvers.Contracts
{
    public interface IArticleResolver
    {
        Task<ArticleModel> CreateArticleAsync(ArticleModel criterias);
        Task<ArticlePageListModel> GetUserArticlesAsync(ArticleFilterModel criterias);
        Task<ArticlePageListModel> GetArticlesAsync(ArticleFilterModel criterias);
    }
}
