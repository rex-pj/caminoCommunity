using Camino.Service.Projections.Feed;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Feeds.Contracts
{
    public interface IFeedBusiness
    {
        Task<BasePageList<FeedProjection>> GetAsync(FeedFilter filter);
    }
}
