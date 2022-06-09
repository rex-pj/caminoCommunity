using Camino.Application.Contracts.AppServices.Feeds.Dtos;

namespace Camino.Application.Contracts.AppServices.Feeds
{
    public interface IFeedAppService
    {
        Task<BasePageList<FeedResult>> GetAsync(FeedFilter filter);
        Task<GroupOfFeedResult> GetInGroupAsync(FeedFilter filter);
        Task<GroupOfFeedResult> LiveSearchInGroupAsync(FeedFilter filter);
        Task PopulateDetailsAsync(GroupOfFeedResult feedInGroup);
        Task PopulateDetailsAsync(IList<FeedResult> feeds);
    }
}
