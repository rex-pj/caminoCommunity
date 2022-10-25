using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Feed.Api.GraphQL.Resolvers.Contracts;
using Module.Feed.Api.Models;
using System.Threading.Tasks;

namespace Module.Feed.Api.GraphQL.Mutations
{
    [ExtendObjectType("Query")]
    public class SearchQueries : BaseQueries
    {
        public async Task<AdvancedSearchResultModel> AdvancedSearchAsync([Service] ISearchResolver farmResolver, FeedFilterModel criterias)
        {
            return await farmResolver.AdvancedSearchAsync(criterias);
        }

        public async Task<AdvancedSearchResultModel> LiveSearchAsync([Service] ISearchResolver farmResolver, FeedFilterModel criterias)
        {
            return await farmResolver.LiveSearchAsync(criterias);
        }
    }
}
