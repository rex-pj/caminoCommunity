using Camino.Framework.Models;
using Module.Api.Content.Models;
using System.Threading.Tasks;

namespace Module.Api.Content.GraphQL.Resolvers.Contracts
{
    public interface IArticleResolver
    {
        Task<ArticleModel> CreateArticleAsync(ArticleModel criterias);
        Task<PageListModel<ArticleModel>> GetUserArticlesAsync(ArticleFilterModel criterias);
    }
}
