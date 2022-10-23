using Camino.Application.Contracts.AppServices.Feeds;
using Camino.Application.Contracts.AppServices.Feeds.Dtos;
using Camino.Infrastructure.GraphQL.Resolvers;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Enums;
using Microsoft.Extensions.Options;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using Module.Api.Feed.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Resolvers
{
    public class SearchResolver : BaseResolver, ISearchResolver
    {
        private readonly IFeedAppService _feedAppService;
        private readonly IFeedModelService _feedModelService;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly PagerOptions _pagerOptions;

        public SearchResolver(IFeedAppService feedAppService, IFeedModelService feedModelService,
              IUserManager<ApplicationUser> userManager, IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _feedAppService = feedAppService;
            _feedModelService = feedModelService;
            _userManager = userManager;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<AdvancedSearchResultModel> LiveSearchAsync(FeedFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FeedFilterModel();
            }

            var groupOfSearch = await _feedAppService.LiveSearchInGroupAsync(new FeedFilter()
            {
                Page = criterias.Page,
                PageSize = _pagerOptions.LiveSearchPageSize,
                Keyword = criterias.Search
            });
            return new AdvancedSearchResultModel
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

        public async Task<AdvancedSearchResultModel> AdvancedSearchAsync(FeedFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new FeedFilterModel();
            }

            var currentDateTime = DateTime.UtcNow;
            DateTime? createdDateFrom = criterias.HoursCreatedFrom.HasValue ? currentDateTime.AddHours(-criterias.HoursCreatedFrom.Value) : null;
            DateTime? createdDateTo = criterias.HoursCreatedTo.HasValue ? currentDateTime.AddHours(-criterias.HoursCreatedTo.Value) : null;
            var filter = new FeedFilter
            {
                Page = criterias.Page,
                PageSize = _pagerOptions.AdvancedSearchPageSize,
                Keyword = criterias.Search,
                FilterType = criterias.FilterType.HasValue ? (FeedFilterTypes)criterias.FilterType : null,
                CreatedDateFrom = createdDateFrom,
                CreatedDateTo = createdDateTo
            };

            if (!string.IsNullOrEmpty(criterias.UserIdentityId))
            {
                filter.CreatedById = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            }

            var groupOfSearch = await _feedAppService.GetInGroupAsync(filter);
            var searchResult = new AdvancedSearchResultModel
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
                Page = filter.Page
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
