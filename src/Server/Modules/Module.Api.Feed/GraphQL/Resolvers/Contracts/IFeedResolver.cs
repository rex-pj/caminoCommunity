using Module.Api.Feed.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Resolvers.Contracts
{
    public interface IFeedResolver
    {
        Task<FeedPageListModel> GetUserFeedsAsync(ClaimsPrincipal claimsPrincipal, FeedFilterModel criterias);
        Task<FeedPageListModel> GetFeedsAsync(FeedFilterModel criterias);
    }
}
