using Camino.Core.Contracts.IdentityManager;
using Camino.Core.Contracts.Services.Feeds;
using Camino.Core.Domain.Identities;
using Camino.Framework.GraphQL.Resolvers;
using Camino.Shared.Requests.Filters;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using Module.Api.Feed.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Resolvers
{
    public class SearchResolver: BaseResolver, ISearchResolver
    {
        private readonly IFeedService _feedService;
        private readonly IFeedModelService _feedModelService;

        public SearchResolver(ISessionContext sessionContext, IFeedService feedService, IFeedModelService feedModelService)
            : base(sessionContext)
        {
            _feedService = feedService;
            _feedModelService = feedModelService;
        }

        public async Task<SearchInGroupResultModel> LiveSearchAsync(FeedFilterModel criterias)
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

            var groupOfSearch = await _feedService.LiveSearchInGroupAsync(filterRequest);
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
    }
}
