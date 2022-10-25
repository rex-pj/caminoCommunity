using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Feed.Api.GraphQL.Resolvers.Contracts;
using Module.Feed.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Feed.Api.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class FeedQueries : BaseQueries
    {
        public async Task<FeedPageListModel> GetUserFeedsAsync(ClaimsPrincipal claimsPrincipal, [Service] IFeedResolver farmResolver, FeedFilterModel criterias)
        {
            return await farmResolver.GetUserFeedsAsync(claimsPrincipal, criterias);
        }

        public async Task<FeedPageListModel> GetFeedsAsync([Service] IFeedResolver farmResolver, FeedFilterModel criterias)
        {
            return await farmResolver.GetFeedsAsync(criterias);
        }
    }
}
