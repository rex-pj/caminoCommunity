using Module.Api.Feed.Models;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Resolvers.Contracts
{
    public interface ISearchResolver
    {
        Task<AdvancedSearchResultModel> LiveSearchAsync(FeedFilterModel criterias);
        Task<AdvancedSearchResultModel> AdvancedSearchAsync(FeedFilterModel criterias);
    }
}
