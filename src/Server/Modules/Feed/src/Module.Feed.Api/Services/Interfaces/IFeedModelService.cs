using Camino.Application.Contracts.AppServices.Feeds.Dtos;
using Module.Feed.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Feed.Api.Services.Interfaces
{
    public interface IFeedModelService
    {
        Task<IList<FeedModel>> MapFeedsResultToModelAsync(IEnumerable<FeedResult> feedResults);
    }
}
