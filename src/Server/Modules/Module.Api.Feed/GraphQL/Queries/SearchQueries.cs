using Camino.Framework.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Mutations
{
    [ExtendObjectType("Query")]
    public class SearchQueries : BaseQueries
    {
        public async Task<SearchInGroupResultModel> AdvancedSearchAsync([Service] ISearchResolver farmResolver, FeedFilterModel criterias)
        {
            return await farmResolver.AdvancedSearchAsync(criterias);
        }
    }
}
