using Module.Api.Content.Models;
using System.Threading.Tasks;

namespace Module.Api.Content.GraphQL.Resolvers.Contracts
{
    public interface IArticleResolver
    {
        Task<ArticleModel> CreateArticleAsync(ArticleModel criterias);
        Task<ArticlePageListModel> GetUserArticlesAsync(ArticleFilterModel criterias);
    }
}
