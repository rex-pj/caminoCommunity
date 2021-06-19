using Camino.Core.Domain.Identities;
using Module.Api.Feed.Models;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Resolvers.Contracts
{
    public interface IFeedResolver
    {
        Task<FeedPageListModel> GetUserFeedsAsync(ApplicationUser currentUser, FeedFilterModel criterias);
        Task<FeedPageListModel> GetFeedsAsync(FeedFilterModel criterias);
    }
}
