using Camino.Core.Contracts.IdentityManager;
using Camino.Core.Contracts.Services.Feeds;
using Camino.Core.Domain.Identities;
using Camino.Framework.GraphQL.Resolvers;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Filters;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using Module.Api.Feed.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Resolvers
{
    public class SearchResolver : BaseResolver, ISearchResolver
    {
        private readonly IFeedService _feedService;
        private readonly IFeedModelService _feedModelService;
        private readonly IUserManager<ApplicationUser> _userManager;

        public SearchResolver(ISessionContext sessionContext, IFeedService feedService, IFeedModelService feedModelService,
              IUserManager<ApplicationUser> userManager)
            : base(sessionContext)
        {
            _feedService = feedService;
            _feedModelService = feedModelService;
            _userManager = userManager;
        }

        public async Task<SearchInGroupResultModel> LiveSearchAsync(FeedFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FeedFilterModel();
            }

            var groupOfSearch = await _feedService.LiveSearchInGroupAsync(new FeedFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search
            });
            return new SearchInGroupResultModel
            {
                Articles = await _feedModelService.MapFeedsResultToModelAsync(groupOfSearch.Articles),
                TotalArticle = groupOfSearch.TotalArticle,
                TotalArticlePage = groupOfSearch.TotalArticlePage,
                Products = await _feedModelService.MapFeedsResultToModelAsync(groupOfSearch.Products),
                TotalProduct = groupOfSearch.TotalProduct,
                TotalProductPage = groupOfSearch.TotalProductPage,
                Farms = await _feedModelService.MapFeedsResultToModelAsync(groupOfSearch.Farms),
                TotalFarm = groupOfSearch.TotalFarm,
                TotalFarmPage = groupOfSearch.TotalFarmPage,
                Users = await _feedModelService.MapFeedsResultToModelAsync(groupOfSearch.Users),
                TotalUser = groupOfSearch.TotalUser,
                TotalUserPage = groupOfSearch.TotalUserPage,
            };
        }

        public async Task<SearchInGroupResultModel> AdvancedSearchAsync(FeedFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FeedFilterModel();
            }

            var currentDateTime = DateTimeOffset.UtcNow;
            DateTimeOffset? createdDateFrom = criterias.HoursCreatedFrom.HasValue ? currentDateTime.AddHours(-criterias.HoursCreatedFrom.Value) : null;
            DateTimeOffset? createdDateTo = criterias.HoursCreatedTo.HasValue ? currentDateTime.AddHours(-criterias.HoursCreatedTo.Value) : null;
            var filter = new FeedFilter
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search,
                FilterType = criterias.FilterType.HasValue ? (FeedFilterType)criterias.FilterType : null,
                CreatedDateFrom = createdDateFrom,
                CreatedDateTo = createdDateTo
            };

            if (!string.IsNullOrEmpty(criterias.UserIdentityId))
            {
                filter.CreatedById = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            }

            var groupOfSearch = await _feedService.SearchInGroupAsync(filter);
            var searchResult = new SearchInGroupResultModel
            {
                Articles = await _feedModelService.MapFeedsResultToModelAsync(groupOfSearch.Articles),
                TotalArticle = groupOfSearch.TotalArticle,
                TotalArticlePage = groupOfSearch.TotalArticlePage,
                Products = await _feedModelService.MapFeedsResultToModelAsync(groupOfSearch.Products),
                TotalProduct = groupOfSearch.TotalProduct,
                TotalProductPage = groupOfSearch.TotalProductPage,
                Farms = await _feedModelService.MapFeedsResultToModelAsync(groupOfSearch.Farms),
                TotalFarm = groupOfSearch.TotalFarm,
                TotalFarmPage = groupOfSearch.TotalFarmPage,
                Users = await _feedModelService.MapFeedsResultToModelAsync(groupOfSearch.Users),
                TotalUser = groupOfSearch.TotalUser,
                TotalUserPage = groupOfSearch.TotalUserPage,
            };

            if (!string.IsNullOrEmpty(criterias.UserIdentityId))
            {
                filter.CreatedById = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
                var createdUser = await _userManager.FindByIdentityIdAsync(criterias.UserIdentityId);
                if (createdUser != null)
                {
                    searchResult.UserFilterByName = createdUser.DisplayName;
                }
            }

            return searchResult;
        }
    }
}
