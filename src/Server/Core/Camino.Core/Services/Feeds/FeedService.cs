using Camino.Core.Contracts.Services.Feeds;
using Camino.Shared.Results.Feed;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Feeds;
using System.Linq;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Utils;

namespace Camino.Services.Feeds
{
    public class FeedService : IFeedService
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IUserRepository _userRepository;

        public FeedService(IFeedRepository feedRepository, IUserRepository userRepository)
        {
            _feedRepository = feedRepository;
            _userRepository = userRepository;
        }

        public async Task<BasePageList<FeedResult>> GetAsync(FeedFilter filter)
        {
            var feedsPageList = await _feedRepository.GetAsync(filter);

            var createdByIds = feedsPageList.Collections.Select(x => x.CreatedById).ToArray();
            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);

            foreach (var feed in feedsPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == feed.CreatedById);
                feed.CreatedByName = createdBy.DisplayName;
                feed.Description = HtmlUtil.TrimHtml(feed.Description);
            }

            return feedsPageList;
        }
    }
}
