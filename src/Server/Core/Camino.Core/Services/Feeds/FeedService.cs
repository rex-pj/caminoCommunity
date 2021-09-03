using Camino.Core.Contracts.Services.Feeds;
using Camino.Shared.Results.Feed;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Feeds;
using System.Linq;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Utils;
using Camino.Shared.Enums;
using Camino.Core.Contracts.Repositories.Articles;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Contracts.Repositories.Farms;
using System.Collections.Generic;
using Camino.Shared.Results.Media;
using Camino.Shared.Results.Identifiers;

namespace Camino.Services.Feeds
{
    public class FeedService : IFeedService
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IFarmPictureRepository _farmPictureRepository;
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly IArticlePictureRepository _articlePictureRepository;

        public FeedService(IFeedRepository feedRepository, IUserRepository userRepository,
            IArticlePictureRepository articlePictureRepository,
            IProductPictureRepository productPictureRepository,
            IFarmPictureRepository farmPictureRepository,
            IUserPhotoRepository userPhotoRepository)
        {
            _feedRepository = feedRepository;
            _userRepository = userRepository;

            _articlePictureRepository = articlePictureRepository;
            _farmPictureRepository = farmPictureRepository;
            _productPictureRepository = productPictureRepository;
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<BasePageList<FeedResult>> GetAsync(FeedFilter filter)
        {
            var feedsPageList = await _feedRepository.GetAsync(filter);

            // Get article pictures
            var articleIds = feedsPageList.Collections.Where(x => x.FeedType == FeedType.Article).Select(x => long.Parse(x.Id));
            var articlePictures = await _articlePictureRepository
                .GetArticlePicturesByArticleIdsAsync(articleIds, new IdRequestFilter<long>(), ArticlePictureType.Thumbnail);

            // Get product pictures
            var productIds = feedsPageList.Collections.Where(x => x.FeedType == FeedType.Product).Select(x => long.Parse(x.Id));
            var productPictures = await _productPictureRepository
                .GetProductPicturesByProductIdsAsync(productIds, new IdRequestFilter<long>(), ProductPictureType.Thumbnail);

            // Get farm pictures
            var farmIds = feedsPageList.Collections.Where(x => x.FeedType == FeedType.Farm).Select(x => long.Parse(x.Id));
            var farmPictures = await _farmPictureRepository
                .GetFarmPicturesByFarmIdsAsync(farmIds, new IdRequestFilter<long>(), FarmPictureType.Thumbnail);

            // Get user pictures
            var userIds = feedsPageList.Collections.Where(x => x.FeedType == FeedType.User).Select(x => long.Parse(x.Id));
            var userPictures = await _userPhotoRepository.GetUserPhotoByUserIdsAsync(userIds, UserPictureType.Avatar);

            // Get created by user's photos
            var createdByIds = feedsPageList.Collections.Select(x => x.CreatedById);
            var createdByUserPictures = await _userPhotoRepository.GetUserPhotoByUserIdsAsync(createdByIds, UserPictureType.Avatar);

            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            foreach (var feed in feedsPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == feed.CreatedById);
                feed.CreatedByName = createdBy.DisplayName;
                feed.Description = HtmlUtil.TrimHtml(feed.Description);
                var createdByUserPicture = createdByUserPictures.FirstOrDefault(x => x.UserId == feed.CreatedById);
                if (createdByUserPicture != null)
                {
                    feed.CreatedByPhotoCode = createdByUserPicture.Code;
                }

                switch (feed.FeedType)
                {
                    case FeedType.Article:
                        var articlePicture = articlePictures.FirstOrDefault(x => x.ArticleId == long.Parse(feed.Id));
                        if (articlePicture != null)
                        {
                            feed.PictureId = articlePicture.PictureId.ToString();
                        }
                        break;
                    case FeedType.Product:
                        var productPicture = productPictures.FirstOrDefault(x => x.ProductId == long.Parse(feed.Id));
                        if (productPicture != null)
                        {
                            feed.PictureId = productPicture.PictureId.ToString();
                        }
                        break;
                    case FeedType.Farm:
                        var farmePicture = farmPictures.FirstOrDefault(x => x.FarmId == long.Parse(feed.Id));
                        if (farmePicture != null)
                        {
                            feed.PictureId = farmePicture.PictureId.ToString();
                        }
                        break;
                    default:
                        break;
                }
            }

            return feedsPageList;
        }

        public async Task<SearchInGroupResult> SearchInGroupAsync(FeedFilter filter)
        {
            var groupOfSearch = await _feedRepository.GetInGroupAsync(filter);

            var createdByIds = groupOfSearch.Articles.Select(x => x.CreatedById)
                .Concat(groupOfSearch.Products.Select(x => x.CreatedById))
                .Concat(groupOfSearch.Farms.Select(x => x.CreatedById)).Distinct();
            var createdByPictures = await _userPhotoRepository.GetUserPhotoByUserIdsAsync(createdByIds, UserPictureType.Avatar);
            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            await SetPicturesForSearchResultAsync(groupOfSearch, createdByUsers, createdByPictures);

            return groupOfSearch;
        }

        public async Task<SearchInGroupResult> LiveSearchInGroupAsync(FeedFilter filter)
        {
            var groupOfSearch = await _feedRepository.GetInGroupAsync(filter);
            await SetPicturesForSearchResultAsync(groupOfSearch);

            return groupOfSearch;
        }

        private async Task SetPicturesForSearchResultAsync(SearchInGroupResult groupOfSearch, IEnumerable<UserResult> createdByUsers = null, IList<UserPhotoResult> createdByPictures = null)
        {
            // Get articles pictures
            var articleIds = groupOfSearch.Articles.Select(x => long.Parse(x.Id));
            var articlePictures = await _articlePictureRepository
                .GetArticlePicturesByArticleIdsAsync(articleIds, new IdRequestFilter<long>(), ArticlePictureType.Thumbnail);
            foreach (var article in groupOfSearch.Articles)
            {
                SetFeedCreatedPicture(article, createdByPictures);
                SetFeedCreatedByName(article, createdByUsers);

                var articlePicture = articlePictures.FirstOrDefault(x => x.ArticleId == long.Parse(article.Id));
                if (articlePicture != null)
                {
                    article.PictureId = articlePicture.PictureId.ToString();
                }
            }

            // Get product pictures
            var productIds = groupOfSearch.Products.Select(x => long.Parse(x.Id));
            var productPictures = await _productPictureRepository
                .GetProductPicturesByProductIdsAsync(productIds, new IdRequestFilter<long>(), ProductPictureType.Thumbnail);
            foreach (var product in groupOfSearch.Products)
            {
                SetFeedCreatedPicture(product, createdByPictures);
                SetFeedCreatedByName(product, createdByUsers);

                var productPicture = productPictures.FirstOrDefault(x => x.ProductId == long.Parse(product.Id));
                if (productPicture != null)
                {
                    product.PictureId = productPicture.PictureId.ToString();
                }
            }

            // Get farm pictures
            var farmIds = groupOfSearch.Farms.Select(x => long.Parse(x.Id));
            var farmPictures = await _farmPictureRepository
                .GetFarmPicturesByFarmIdsAsync(farmIds, new IdRequestFilter<long>(), FarmPictureType.Thumbnail);
            foreach (var farm in groupOfSearch.Farms)
            {
                SetFeedCreatedPicture(farm, createdByPictures);
                SetFeedCreatedByName(farm, createdByUsers);

                var farmPicture = farmPictures.FirstOrDefault(x => x.FarmId == long.Parse(farm.Id));
                if (farmPicture != null)
                {
                    farm.PictureId = farmPicture.PictureId.ToString();
                }
            }

            // Get user avatar pictures
            var userIds = groupOfSearch.Users.Select(x => long.Parse(x.Id));
            var userPictures = await _userPhotoRepository.GetUserPhotoByUserIdsAsync(userIds, UserPictureType.Avatar);
            foreach (var user in groupOfSearch.Users)
            {
                var userPicture = userPictures.FirstOrDefault(x => x.UserId == long.Parse(user.Id));
                if (userPicture != null)
                {
                    user.PictureId = userPicture.Code;
                }
            }
        }

        private void SetFeedCreatedByName(FeedResult feed,IEnumerable<UserResult> createdByUsers = null)
        {
            if (createdByUsers == null || !createdByUsers.Any())
            {
                return;
            }

            var user = createdByUsers.FirstOrDefault(x => x.Id == feed.CreatedById);
            if (user != null)
            {
                feed.CreatedByName = user.DisplayName;
            }
        }

        private void SetFeedCreatedPicture(FeedResult feed, IList<UserPhotoResult> createdByPictures = null)
        {
            if(createdByPictures== null || !createdByPictures.Any())
            {
                return;
            }

            var createdByUserPicture = createdByPictures.FirstOrDefault(x => x.UserId == feed.CreatedById);
            if (createdByUserPicture != null)
            {
                feed.CreatedByPhotoCode = createdByUserPicture.Code;
            }
        }
    }
}
