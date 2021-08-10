using Camino.Framework.GraphQL.Resolvers;
using Camino.Core.Domain.Identities;
using Camino.Core.Contracts.Services.Feeds;
using Camino.Shared.Requests.Filters;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Contracts.IdentityManager;
using Module.Api.Feed.Services.Interfaces;

namespace Module.Api.Feed.GraphQL.Resolvers
{
    public class FeedResolver : BaseResolver, IFeedResolver
    {
        private readonly IFeedService _feedService;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IFeedModelService _feedModelService;

        public FeedResolver(ISessionContext sessionContext, IFeedService feedService, IUserManager<ApplicationUser> userManager,
            IFeedModelService feedModelService)
            : base(sessionContext)
        {
            _feedService = feedService;
            _userManager = userManager;
            _feedModelService = feedModelService;
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
                var feeds = await _feedModelService.MapFeedsResultToModelAsync(feedPageList.Collections);

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
                var feeds = await _feedModelService.MapFeedsResultToModelAsync(feedPageList.Collections);

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
    }
}
