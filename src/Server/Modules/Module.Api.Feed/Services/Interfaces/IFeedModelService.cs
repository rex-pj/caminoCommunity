using Camino.Application.Contracts.AppServices.Feeds.Dtos;
using Module.Api.Feed.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Feed.Services.Interfaces
{
    public interface IFeedModelService
    {
        Task<IList<FeedModel>> MapFeedsResultToModelAsync(IEnumerable<FeedResult> feedResults);
    }
}
