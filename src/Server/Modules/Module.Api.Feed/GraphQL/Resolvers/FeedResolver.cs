using Camino.Framework.GraphQL.Resolvers;
using Camino.Core.Domain.Identities;
using Camino.Core.Contracts.Services.Feeds;
using Camino.Shared.Results.Feed;
using Camino.Shared.Requests.Filters;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.IdentityManager;

namespace Module.Api.Feed.GraphQL.Resolvers
{
    public class FeedResolver : BaseResolver, IFeedResolver
    {
        private readonly IFeedService _feedService;
        private readonly IUserManager<ApplicationUser> _userManager;

        public FeedResolver(ISessionContext sessionContext, IFeedService feedService, IUserManager<ApplicationUser> userManager)
            : base(sessionContext)
        {
            _feedService = feedService;
            _userManager = userManager;
        }

        public async Task<FeedPageListModel> GetUserFeedsAsync(ApplicationUser currentUser, FeedFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FeedFilterModel();
            }

            if (string.IsNullOrEmpty(criterias.UserIdentityId))
            {
                return new FeedPageListModel(new List<FeedModel>())
                {
                    Filter = criterias
                };
            }

            var userId = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            var filterRequest = new FeedFilter
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search,
                CreatedById = userId,
                CanGetInactived = currentUser.Id == userId
            };

            try
            {
                var feedPageList = await _feedService.GetAsync(filterRequest);
                var feeds = await MapFeedsResultToModelAsync(feedPageList.Collections);

                var feedPage = new FeedPageListModel(feeds)
                {
                    Filter = criterias,
                    TotalPage = feedPageList.TotalPage,
                    TotalResult = feedPageList.TotalResult
                };

                return feedPage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FeedPageListModel> GetFeedsAsync(FeedFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FeedFilterModel();
            }

            var filterRequest = new FeedFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search
            };

            try
            {
                var feedPageList = await _feedService.GetAsync(filterRequest);
                var feeds = await MapFeedsResultToModelAsync(feedPageList.Collections);

                var feedPage = new FeedPageListModel(feeds)
                {
                    Filter = criterias,
                    TotalPage = feedPageList.TotalPage,
                    TotalResult = feedPageList.TotalResult
                };

                return feedPage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<IList<FeedModel>> MapFeedsResultToModelAsync(IEnumerable<FeedResult> feedResults)
        {
            var feeds = feedResults.Select(x => new FeedModel()
            {
                Address = x.Address,
                CreatedById = x.CreatedById,
                CreatedByName = x.CreatedByName,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                FeedType = (int)x.FeedType,
                Id = x.Id,
                Name = x.Name,
                PictureId = x.PictureId,
                Price = x.Price,
                CreatedByPhotoCode = x.CreatedByPhotoCode
            }).ToList();

            foreach (var feed in feeds)
            {
                feed.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(feed.CreatedById);
                if (!string.IsNullOrEmpty(feed.Description) && feed.Description.Length >= 150)
                {
                    feed.Description = $"{feed.Description.Substring(0, 150)}...";
                }
            }

            return feeds;
        }
    }
}
