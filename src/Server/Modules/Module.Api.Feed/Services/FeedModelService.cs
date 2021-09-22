﻿using Camino.Core.Contracts.IdentityManager;
using Camino.Core.Domain.Identities;
using Camino.Core.Utils;
using Camino.Shared.Enums;
using Camino.Shared.Results.Feed;
using Module.Api.Feed.Models;
using Module.Api.Feed.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Api.Feed.Services
{
    public class FeedModelService : IFeedModelService
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        public FeedModelService(IUserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<FeedModel>> MapFeedsResultToModelAsync(IEnumerable<FeedResult> feedResults)
        {
            var feeds = feedResults.Select(x => new FeedModel()
            {
                Address = x.Address,
                CreatedById = x.CreatedById,
                CreatedByName = x.CreatedByName,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                FeedType = (int)x.FeedType,
                Id = x.Id.ToString(),
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

                if (feed.FeedType == FeedType.User.GetCode())
                {
                    feed.Id = await _userManager.EncryptUserIdAsync(long.Parse(feed.Id));
                }
            }

            return feeds;
        }
    }
}