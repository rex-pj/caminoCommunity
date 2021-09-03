using Camino.Shared.Results.Feed;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Feeds
{
    public interface IFeedService
    {
        Task<BasePageList<FeedResult>> GetAsync(FeedFilter filter);
        Task<SearchInGroupResult> SearchInGroupAsync(FeedFilter filter);
        Task<SearchInGroupResult> LiveSearchInGroupAsync(FeedFilter filter);
    }
}
