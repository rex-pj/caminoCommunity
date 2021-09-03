using Camino.Shared.Results.Feed;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Feeds
{
    public interface IFeedRepository
    {
        Task<BasePageList<FeedResult>> GetAsync(FeedFilter filter);
        Task<SearchInGroupResult> GetInGroupAsync(FeedFilter filter);
    }
}
