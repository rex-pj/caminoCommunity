using Module.Api.Feed.Models;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Resolvers.Contracts
{
    public interface IFeedResolver
    {
        Task<FeedPageListModel> GetUserFeedsAsync(FeedFilterModel criterias);
    }
}
