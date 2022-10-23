using Camino.Infrastructure.GraphQL.Resolvers;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Module.Api.Feed.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Camino.Application.Contracts.AppServices.Feeds;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Core;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts.AppServices.Feeds.Dtos;

namespace Module.Api.Feed.GraphQL.Resolvers
{
    public class FeedResolver : BaseResolver, IFeedResolver
    {
        private readonly IFeedAppService _feedAppService;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IFeedModelService _feedModelService;
        private readonly PagerOptions _pagerOptions;

        public FeedResolver(IFeedAppService feedAppService, IUserManager<ApplicationUser> userManager,
            IFeedModelService feedModelService, IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _feedAppService = feedAppService;
            _userManager = userManager;
            _feedModelService = feedModelService;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<FeedPageListModel> GetUserFeedsAsync(ClaimsPrincipal claimsPrincipal, FeedFilterModel criterias)
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

            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var userId = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            var filterRequest = new FeedFilter
            {
                Page = criterias.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = criterias.Search,
                CreatedById = userId,
                CanGetInactived = currentUserId == userId
            };

            try
            {
                var feedPageList = await _feedAppService.GetAsync(filterRequest);
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
                PageSize = _pagerOptions.PageSize,
                Keyword = criterias.Search
            };

            try
            {
                var feedPageList = await _feedAppService.GetAsync(filterRequest);
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
