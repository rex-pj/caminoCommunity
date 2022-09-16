using Camino.Core.Contracts.Repositories.Users;
using Camino.Shared.Enums;
using Camino.Application.Contracts;
using Camino.Shared.Utils;
using Camino.Core.Domains;
using Camino.Core.Domains.Users;
using Camino.Core.Domains.Articles;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Farms;
using Camino.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains.Media;
using Camino.Application.Contracts.AppServices.Feeds;
using Camino.Application.Contracts.AppServices.Articles;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Application.Contracts.AppServices.Feeds.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace Camino.Application.AppServices.Feeds
{
    public class FeedAppService : IFeedAppService, IScopedDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserPhotoRepository _userPhotoRepository;

        private readonly IArticlePictureAppService _articlePictureAppService;
        private readonly IProductPictureAppService _productPictureAppService;
        private readonly IFarmPictureAppService _farmPictureAppService;
        private readonly IEntityRepository<Article> _articleEntityRepository;
        private readonly IEntityRepository<Product> _productEntityRepository;
        private readonly IEntityRepository<Farm> _farmEntityRepository;
        private readonly IEntityRepository<ProductPrice> _productPriceEntityRepository;
        private readonly IEntityRepository<User> _userEntityRepository;
        private readonly int _articleDeletedStatus;
        private readonly int _articleInactivedStatus;
        private readonly int _productDeletedStatus;
        private readonly int _productInactivedStatus;
        private readonly int _farmDeletedStatus;
        private readonly int _farmInactivedStatus;
        private readonly int _userActivedStatus;

        public FeedAppService(IUserRepository userRepository,
            IUserPhotoRepository userPhotoRepository,
            IProductPictureAppService productPictureAppService,
            IArticlePictureAppService articlePictureAppService,
            IFarmPictureAppService farmPictureAppService,
            IEntityRepository<Article> articleEntityRepository,
            IEntityRepository<Product> productEntityRepository,
            IEntityRepository<Farm> farmEntityRepository,
            IEntityRepository<User> userEntityRepository,
            IEntityRepository<ProductPrice> productPriceEntityRepository)
        {
            _userRepository = userRepository;
            _userPhotoRepository = userPhotoRepository;

            _productPictureAppService = productPictureAppService;
            _farmPictureAppService = farmPictureAppService;
            _articlePictureAppService = articlePictureAppService;

            _articleEntityRepository = articleEntityRepository;
            _productEntityRepository = productEntityRepository;
            _farmEntityRepository = farmEntityRepository;
            _userEntityRepository = userEntityRepository;
            _productPriceEntityRepository = productPriceEntityRepository;

            _articleDeletedStatus = ArticleStatuses.Deleted.GetCode();
            _articleInactivedStatus = ArticleStatuses.Inactived.GetCode();
            _productDeletedStatus = ProductStatuses.Deleted.GetCode();
            _productInactivedStatus = ProductStatuses.Inactived.GetCode();
            _farmDeletedStatus = FarmStatuses.Deleted.GetCode();
            _farmInactivedStatus = FarmStatuses.Inactived.GetCode();
            _userActivedStatus = UserStatuses.Actived.GetCode();
        }

        public async Task<BasePageList<FeedResult>> GetAsync(FeedFilter filter)
        {
            var articleQuery = _articleEntityRepository.Get(x => (x.StatusId == _articleDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _articleInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _articleDeletedStatus && x.StatusId != _articleInactivedStatus));

            var productQuery = _productEntityRepository.Get(x => (x.StatusId == _productDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _productInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _productDeletedStatus && x.StatusId != _productInactivedStatus));

            var farmQuery = _farmEntityRepository.Get(x => (x.StatusId == _farmDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _farmInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _farmDeletedStatus && x.StatusId != _farmInactivedStatus));

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                filter.Keyword = filter.Keyword.ToLower();

                articleQuery = articleQuery.Where(x => x.Name.ToLower().Contains(filter.Keyword) || x.Description.Contains(filter.Keyword));
                productQuery = productQuery.Where(x => x.Name.ToLower().Contains(filter.Keyword) || x.Description.Contains(filter.Keyword));
                farmQuery = farmQuery.Where(x => x.Name.ToLower().Contains(filter.Keyword) || x.Description.Contains(filter.Keyword));
            }

            if (filter.CreatedById.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedById == filter.CreatedById);
                productQuery = productQuery.Where(x => x.CreatedById == filter.CreatedById);
                farmQuery = farmQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
                productQuery = productQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
                farmQuery = farmQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom);
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom);
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom);
            }

            var articleFeeds = articleQuery
                .AsNoTracking()
                .Select(article => new FeedResult
                {
                    CreatedById = article.CreatedById,
                    CreatedDate = article.CreatedDate,
                    Description = article.Description == null || article.Description == "" ? article.Content : article.Description,
                    Id = article.Id.ToString(),
                    Name = article.Name,
                    FeedType = FeedTypes.Article,
                    Address = null,
                });

            var productFeeds = productQuery
                .AsNoTracking()
                .Select(product => new FeedResult
                {
                    CreatedById = product.CreatedById,
                    CreatedDate = product.CreatedDate,
                    Description = product.Description,
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    FeedType = FeedTypes.Product,
                    Address = null
                });

            var farmFeeds = farmQuery
                .AsNoTracking()
                .Select(farm => new FeedResult
                {
                    CreatedById = farm.CreatedById,
                    CreatedDate = farm.CreatedDate,
                    Description = farm.Description,
                    Id = farm.Id.ToString(),
                    Name = farm.Name,
                    FeedType = FeedTypes.Farm,
                    Address = farm.Address,
                });

            var articleFilteredCount = await articleFeeds.Select(x => x.Id).CountAsync();
            var productFilteredCount = await productFeeds.Select(x => x.Id).CountAsync();
            var farmFilteredCount = await farmFeeds.Select(x => x.Id).CountAsync();
            var filteredNumber = articleFilteredCount + productFilteredCount + farmFilteredCount;

            var feeds = articleFeeds
                .Union(productFeeds)
                .Union(farmFeeds)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize)
                .ToList();

            await PopulateDetailsAsync(feeds);

            var result = new BasePageList<FeedResult>(feeds)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public async Task PopulateDetailsAsync(IList<FeedResult> feeds)
        {
            // Get article pictures
            var articleIds = feeds.Where(x => x.FeedType == FeedTypes.Article).Select(x => long.Parse(x.Id));
            var articlePictures = await _articlePictureAppService
                .GetListByArticleIdsAsync(articleIds, new IdRequestFilter<long>(), ArticlePictureTypes.Thumbnail);

            // Get product pictures
            var productIds = feeds.Where(x => x.FeedType == FeedTypes.Product).Select(x => long.Parse(x.Id));
            var productPictures = await _productPictureAppService
                .GetListByProductIdsAsync(productIds, new IdRequestFilter<long>(), ProductPictureTypes.Thumbnail);
            // Get product price
            var productPrices = _productPriceEntityRepository.Get(x => productIds.Contains(x.ProductId) && x.IsCurrent);

            // Get farm pictures
            var farmIds = feeds.Where(x => x.FeedType == FeedTypes.Farm).Select(x => long.Parse(x.Id));
            var farmPictures = await _farmPictureAppService
                .GetListByFarmIdsAsync(farmIds, new IdRequestFilter<long>(), FarmPictureTypes.Thumbnail);

            // Get user pictures
            var userIds = feeds.Where(x => x.FeedType == FeedTypes.User).Select(x => long.Parse(x.Id));
            var userPictures = await _userPhotoRepository.GetListByUserIdsAsync(userIds, UserPictureTypes.Avatar);

            // Get created by user's photos
            var createdByIds = feeds.Where(x => x.CreatedById.HasValue).Select(x => x.CreatedById.Value);
            var createdByUserPictures = await _userPhotoRepository.GetListByUserIdsAsync(createdByIds, UserPictureTypes.Avatar);

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            foreach (var feed in feeds)
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
                    case FeedTypes.Article:
                        var articlePicture = articlePictures.FirstOrDefault(x => x.ArticleId == long.Parse(feed.Id));
                        if (articlePicture != null)
                        {
                            feed.PictureId = articlePicture.PictureId.ToString();
                        }
                        break;
                    case FeedTypes.Product:
                        var productPicture = productPictures.FirstOrDefault(x => x.ProductId == long.Parse(feed.Id));
                        if (productPicture != null)
                        {
                            feed.PictureId = productPicture.PictureId.ToString();
                        }

                        var productPrice = productPrices.FirstOrDefault(x => x.ProductId == long.Parse(feed.Id));
                        if (productPrice != null)
                        {
                            feed.Price = productPrice.Price;
                        }
                        break;
                    case FeedTypes.Farm:
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
        }

        public async Task<GroupOfFeedResult> GetInGroupAsync(FeedFilter filter)
        {
            var canFilterArticle = !filter.FilterType.HasValue || filter.FilterType == FeedFilterTypes.Article;
            var articleQuery = _articleEntityRepository.Get(x => canFilterArticle).Where(x => (x.StatusId == _articleDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _articleInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _articleDeletedStatus && x.StatusId != _articleInactivedStatus));

            var canFilterProduct = !filter.FilterType.HasValue || filter.FilterType == FeedFilterTypes.Product;
            var productQuery = _productEntityRepository.Get(x => canFilterProduct).Where(x => (x.StatusId == _productDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _productInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _productDeletedStatus && x.StatusId != _productInactivedStatus));

            var canFilterFarm = !filter.FilterType.HasValue || filter.FilterType == FeedFilterTypes.Farm;
            var farmQuery = _farmEntityRepository.Get(x => canFilterFarm).Where(x => (x.StatusId == _farmDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _farmInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _farmDeletedStatus && x.StatusId != _farmInactivedStatus));

            var canFilterUser = !filter.FilterType.HasValue || filter.FilterType == FeedFilterTypes.User;
            var userQuery = _userEntityRepository.Get(x => canFilterUser).Where(x => x.IsEmailConfirmed && (filter.CanGetInactived || x.StatusId == _userActivedStatus));
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                filter.Keyword = filter.Keyword.ToLower();
                if (canFilterArticle)
                {
                    articleQuery = articleQuery.Where(x => x.Name.ToLower().Contains(filter.Keyword) || x.Description.ToLower().Contains(filter.Keyword));
                }

                if (canFilterProduct)
                {
                    productQuery = productQuery.Where(x => x.Name.ToLower().Contains(filter.Keyword) || x.Description.ToLower().Contains(filter.Keyword));
                }

                if (canFilterFarm)
                {
                    farmQuery = farmQuery.Where(x => x.Name.ToLower().Contains(filter.Keyword) || x.Description.ToLower().Contains(filter.Keyword));
                }

                if (canFilterUser)
                {
                    userQuery = userQuery.Where(x => x.Lastname.ToLower().Contains(filter.Keyword)
                    || x.Firstname.ToLower().Contains(filter.Keyword)
                    || (x.Lastname + " " + x.Firstname).ToLower().Contains(filter.Keyword)
                    || (x.Firstname + " " + x.Lastname).ToLower().Contains(filter.Keyword));
                }
            }

            if (filter.CreatedById.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedById == filter.CreatedById);
                productQuery = productQuery.Where(x => x.CreatedById == filter.CreatedById);
                farmQuery = farmQuery.Where(x => x.CreatedById == filter.CreatedById);
                userQuery = userQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
                userQuery = userQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
                productQuery = productQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
                farmQuery = farmQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
                userQuery = userQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom);
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom);
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom);
                userQuery = userQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom);
            }

            var articleFeeds = articleQuery
                .AsNoTracking()
                .Select(article => new FeedResult
                {
                    CreatedById = article.CreatedById,
                    CreatedDate = article.CreatedDate,
                    Description = article.Description == null || article.Description == "" ? article.Content : article.Description,
                    Id = article.Id.ToString(),
                    Name = article.Name,
                    FeedType = FeedTypes.Article,
                    Address = null
                });

            var productFeeds = productQuery
                .AsNoTracking()
                .Select(product => new FeedResult
                {
                    CreatedById = product.CreatedById,
                    CreatedDate = product.CreatedDate,
                    Description = product.Description,
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    FeedType = FeedTypes.Product,
                    Address = null
                });

            var farmFeeds = farmQuery
                .AsNoTracking()
                .Select(farm => new FeedResult
                {
                    CreatedById = farm.CreatedById,
                    CreatedDate = farm.CreatedDate,
                    Description = farm.Description,
                    Id = farm.Id.ToString(),
                    Name = farm.Name,
                    FeedType = FeedTypes.Farm,
                    Address = farm.Address,
                });

            var userFeeds = userQuery
                .AsNoTracking()
                .Select(user => new FeedResult
                {
                    CreatedById = user.CreatedById,
                    CreatedDate = user.CreatedDate,
                    Description = user.Description,
                    Id = user.Id.ToString(),
                    Name = user.Lastname + " " + user.Firstname,
                    FeedType = FeedTypes.User,
                    Address = user.Address,
                });

            var totalUser = await userFeeds.CountAsync();
            var totalFarm = await farmFeeds.CountAsync();
            var totalArticle = await articleFeeds.CountAsync();
            var totalProduct = await productFeeds.CountAsync();

            int page = filter.Page;
            int pageSize = filter.PageSize;
            var articles = await articleFeeds
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
            var products = await productFeeds
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
            var farms = await farmFeeds
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
            var users = await userFeeds
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();

            var result = new GroupOfFeedResult
            {
                Articles = articles,
                TotalArticle = totalArticle,
                TotalArticlePage = (int)Math.Ceiling((double)totalArticle / filter.PageSize),
                Products = products,
                TotalProduct = totalProduct,
                TotalProductPage = (int)Math.Ceiling((double)totalProduct / filter.PageSize),
                Farms = farms,
                TotalFarm = totalFarm,
                TotalFarmPage = (int)Math.Ceiling((double)totalFarm / filter.PageSize),
                Users = users,
                TotalUser = totalUser,
                TotalUserPage = (int)Math.Ceiling((double)totalUser / filter.PageSize),
            };

            await PopulateDetailsAsync(result);
            return result;
        }

        public async Task PopulateDetailsAsync(GroupOfFeedResult feedInGroup)
        {
            var createdByIds = feedInGroup.Articles.Select(x => x.CreatedById)
                .Concat(feedInGroup.Products.Select(x => x.CreatedById))
                .Concat(feedInGroup.Farms.Select(x => x.CreatedById))
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .Distinct();
            var createdByPictures = await _userPhotoRepository.GetListByUserIdsAsync(createdByIds, UserPictureTypes.Avatar);
            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            await SetPicturesForSearchResultAsync(feedInGroup, createdByUsers, createdByPictures);
        }

        public async Task<GroupOfFeedResult> LiveSearchInGroupAsync(FeedFilter filter)
        {
            var groupOfSearch = await GetInGroupAsync(filter);
            await SetPicturesForSearchResultAsync(groupOfSearch);

            return groupOfSearch;
        }

        private async Task SetPicturesForSearchResultAsync(GroupOfFeedResult groupOfSearch, IList<User> createdByUsers = null, IList<UserPhoto> createdByPictures = null)
        {
            // Get articles pictures
            var articleIds = groupOfSearch.Articles.Select(x => long.Parse(x.Id));
            var articlePictures = await _articlePictureAppService
                .GetListByArticleIdsAsync(articleIds, new IdRequestFilter<long>(), ArticlePictureTypes.Thumbnail);
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
            var productPictures = await _productPictureAppService
                .GetListByProductIdsAsync(productIds, new IdRequestFilter<long>(), ProductPictureTypes.Thumbnail);

            // Get product price
            var productPrices = _productPriceEntityRepository.Get(x => productIds.Contains(x.ProductId) && x.IsCurrent);
            foreach (var product in groupOfSearch.Products)
            {
                SetFeedCreatedPicture(product, createdByPictures);
                SetFeedCreatedByName(product, createdByUsers);

                var productPicture = productPictures.FirstOrDefault(x => x.ProductId == long.Parse(product.Id));
                if (productPicture != null)
                {
                    product.PictureId = productPicture.PictureId.ToString();
                }

                var productPrice = productPrices.FirstOrDefault(x => x.ProductId == long.Parse(product.Id));
                if (productPrice != null)
                {
                    product.Price = productPrice.Price;
                }
            }

            // Get farm pictures
            var farmIds = groupOfSearch.Farms.Select(x => long.Parse(x.Id));
            var farmPictures = await _farmPictureAppService
                .GetListByFarmIdsAsync(farmIds, new IdRequestFilter<long>(), FarmPictureTypes.Thumbnail);
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
            var userPictures = await _userPhotoRepository.GetListByUserIdsAsync(userIds, UserPictureTypes.Avatar);
            foreach (var user in groupOfSearch.Users)
            {
                var userPicture = userPictures.FirstOrDefault(x => x.UserId == long.Parse(user.Id));
                if (userPicture != null)
                {
                    user.PictureId = userPicture.Code;
                }
            }
        }

        private void SetFeedCreatedByName(FeedResult feed, IEnumerable<User> createdByUsers = null)
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

        private void SetFeedCreatedPicture(FeedResult feed, IList<UserPhoto> createdByPictures = null)
        {
            if (createdByPictures == null || !createdByPictures.Any())
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
