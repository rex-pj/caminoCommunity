using Module.Feed.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Feed.Api.GraphQL.Resolvers.Contracts
{
    public interface IFeedResolver
    {
        Task<FeedPageListModel> GetUserFeedsAsync(ClaimsPrincipal claimsPrincipal, FeedFilterModel criterias);
        Task<FeedPageListModel> GetFeedsAsync(FeedFilterModel criterias);
    }
}
