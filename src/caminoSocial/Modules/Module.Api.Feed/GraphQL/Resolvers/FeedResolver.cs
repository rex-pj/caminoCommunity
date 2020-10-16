using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Contracts.Core;
using Camino.IdentityManager.Models;
using Camino.Service.Business.Feeds.Contracts;
using Camino.Service.Projections.Filters;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Resolvers
{
    public class FeedResolver : BaseResolver, IFeedResolver
    {
        private readonly IFeedBusiness _feedBusiness;
        private readonly IUserManager<ApplicationUser> _userManager;

        public FeedResolver(SessionState sessionState, IFeedBusiness feedBusiness, IUserManager<ApplicationUser> userManager)
            : base(sessionState)
        {
            _feedBusiness = feedBusiness;
            _userManager = userManager;
        }

        public async Task<FeedPageListModel> GetUserFeedsAsync(FeedFilterModel criterias)
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
            var filterRequest = new FeedFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search,
                CreatedById = userId
            };

            try
            {
                var feedPageList = await _feedBusiness.GetAsync(filterRequest);
                var feeds = feedPageList.Collections.Select(x => new FeedModel()
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

                var feedPage = new FeedPageListModel(feeds)
                {
                    Filter = criterias,
                    TotalPage = feedPageList.TotalPage,
                    TotalResult = feedPageList.TotalResult
                };

                return feedPage;
            }
            catch (Exception e)
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
                var feedPageList = await _feedBusiness.GetAsync(filterRequest);
                var feeds = feedPageList.Collections.Select(x => new FeedModel()
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
                    if (!string.IsNullOrEmpty(feed.Description) && feed.Description.Length >= 150)
                    {
                        feed.Description = $"{feed.Description.Substring(0, 150)}...";
                    }
                }

                var feedPage = new FeedPageListModel(feeds)
                {
                    Filter = criterias,
                    TotalPage = feedPageList.TotalPage,
                    TotalResult = feedPageList.TotalResult
                };

                return feedPage;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
