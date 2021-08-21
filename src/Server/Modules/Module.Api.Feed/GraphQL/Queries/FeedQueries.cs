using Camino.Framework.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Queries
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
