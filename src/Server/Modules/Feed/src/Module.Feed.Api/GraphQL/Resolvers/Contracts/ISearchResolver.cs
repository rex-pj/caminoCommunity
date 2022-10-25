using Module.Feed.Api.Models;
using System.Threading.Tasks;

namespace Module.Feed.Api.GraphQL.Resolvers.Contracts
{
    public interface ISearchResolver
    {
        Task<AdvancedSearchResultModel> LiveSearchAsync(FeedFilterModel criterias);
        Task<AdvancedSearchResultModel> AdvancedSearchAsync(FeedFilterModel criterias);
    }
}
