using Camino.Core.Utils;
using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.Data.Enums;
using Camino.IdentityDAL.Entities;
using Camino.Service.Business.Feeds.Contracts;
using Camino.Service.Projections.Feed;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Service.Business.Feeds
{
    public class FeedBusiness : IFeedBusiness
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<ArticlePicture> _articlePictureRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<FarmPicture> _farmPictureRepository;
        private readonly IRepository<ProductPrice> _productPriceRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserPhoto> _userPhotoRepository;

        public FeedBusiness(IRepository<Article> articleRepository, IRepository<Product> productRepository,
            IRepository<Farm> farmRepository, IRepository<ArticlePicture> articlePictureRepository,
            IRepository<ProductPicture> productPictureRepository, IRepository<FarmPicture> farmPictureRepository,
            IRepository<ProductPrice> productPriceRepository, IRepository<User> userRepository,
            IRepository<UserPhoto> userPhotoRepository)
        {
            _articleRepository = articleRepository;
            _productRepository = productRepository;
            _farmRepository = farmRepository;
            _farmPictureRepository = farmPictureRepository;
            _articlePictureRepository = articlePictureRepository;
            _productPriceRepository = productPriceRepository;
            _productPictureRepository = productPictureRepository;
            _userRepository = userRepository;
            _userRepository = userRepository;
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<BasePageList<FeedProjection>> GetAsync(FeedFilter filter)
        {
            var articleQuery = _articleRepository.Table;
            var productQuery = _productRepository.Table;
            var farmQuery = _farmRepository.Table;
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

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var articlePictureType = (int)ArticlePictureType.Thumbnail;
            var articleFeeds = (from article in articleQuery
                                join articlePic in _articlePictureRepository.Get(x => x.PictureType == articlePictureType)
                                on article.Id equals articlePic.ArticleId into articlePics
                                from ap in articlePics.DefaultIfEmpty()
                                join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                                on article.CreatedById equals pho.CreatedById into photos
                                from photo in photos.DefaultIfEmpty()
                                select new FeedProjection()
                                {
                                    CreatedById = article.CreatedById,
                                    CreatedDate = article.CreatedDate,
                                    Description = string.IsNullOrEmpty(article.Description) ? article.Content : article.Description,
                                    Id = article.Id,
                                    Name = article.Name,
                                    PictureId = ap.PictureId,
                                    FeedType = FeedType.Article,
                                    CreatedByPhotoCode = photo.Code
                                });


            var productPictureType = (int)ProductPictureType.Thumbnail;
            var productFeeds = (from product in productQuery
                                join productPic in _productPictureRepository.Get(x => x.PictureType == productPictureType)
                                on product.Id equals productPic.ProductId into productPics
                                from pp in productPics.DefaultIfEmpty()
                                join pr in _productPriceRepository.Get(x => x.IsCurrent)
                                on product.Id equals pr.ProductId into prices
                                from price in prices.DefaultIfEmpty()
                                join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                                on product.CreatedById equals pho.UserId into photos
                                from photo in photos.DefaultIfEmpty()
                                select new FeedProjection()
                                {
                                    CreatedById = product.CreatedById,
                                    CreatedDate = product.CreatedDate,
                                    Description = product.Description,
                                    Id = product.Id,
                                    Name = product.Name,
                                    PictureId = pp.PictureId,
                                    FeedType = FeedType.Product,
                                    Price = price != null ? price.Price : 0,
                                    CreatedByPhotoCode = photo.Code
                                });

            var farmPictureType = (int)FarmPictureType.Thumbnail;
            var farmFeeds = (from farm in farmQuery
                             join farmPic in _farmPictureRepository.Get(x => x.PictureType == farmPictureType)
                             on farm.Id equals farmPic.FarmId into farmPics
                             from fp in farmPics.DefaultIfEmpty()
                             join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                             on farm.CreatedById equals pho.UserId into photos
                             from photo in photos.DefaultIfEmpty()
                             select new FeedProjection()
                             {
                                 CreatedById = farm.CreatedById,
                                 CreatedDate = farm.CreatedDate,
                                 Description = farm.Description,
                                 Id = farm.Id,
                                 Name = farm.Name,
                                 PictureId = fp.PictureId,
                                 FeedType = FeedType.Farm,
                                 Address = farm.Address,
                                 CreatedByPhotoCode = photo.Code
                             });

            var feedQuery = articleFeeds
                .UnionAll(productFeeds)
                .UnionAll(farmFeeds);

            var filteredNumber = await feedQuery.CountAsync();

            articleFeeds = articleFeeds.Take(filter.PageSize);
            productFeeds = productFeeds.Take(filter.PageSize);
            farmFeeds = farmFeeds.Take(filter.PageSize);
            
            feedQuery = articleFeeds
                .UnionAll(productFeeds)
                .UnionAll(farmFeeds)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize);

            var feeds = await feedQuery.ToListAsync();

            var createdByIds = feeds.Select(x => x.CreatedById).ToArray();
            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();

            foreach (var feed in feeds)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == feed.CreatedById);
                feed.CreatedByName = createdBy.DisplayName;
                feed.Description = HtmlUtil.TrimHtml(feed.Description);
            }

            var result = new BasePageList<FeedProjection>(feeds)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }
    }
}
