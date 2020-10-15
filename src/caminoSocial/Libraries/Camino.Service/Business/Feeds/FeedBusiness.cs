using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.Data.Enums;
using Camino.Service.Business.Feeds.Contracts;
using Camino.Service.Projections.Feed;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using LinqToDB;
using System;
using System.Collections.Generic;
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

        public FeedBusiness(IRepository<Article> articleRepository, IRepository<Product> productRepository,
            IRepository<Farm> farmRepository, IRepository<ArticlePicture> articlePictureRepository,
            IRepository<ProductPicture> productPictureRepository, IRepository<FarmPicture> farmPictureRepository,
            IRepository<ProductPrice> productPriceRepository)
        {
            _articleRepository = articleRepository;
            _productRepository = productRepository;
            _farmRepository = farmRepository;
            _farmPictureRepository = farmPictureRepository;
            _articlePictureRepository = articlePictureRepository;
            _productPriceRepository = productPriceRepository;
            _productPictureRepository = productPictureRepository;
        }

        public async Task<BasePageList<FeedProjection>> GetAsync(FeedFilter filter)
        {
            var articlePictureType = (int)ArticlePictureType.Thumbnail;
            var articleFeeds = (from article in _articleRepository.Table
                                join articlePic in _articlePictureRepository.Get(x => x.PictureType == articlePictureType)
                                on article.Id equals articlePic.ArticleId into articlePics
                                from ap in articlePics.DefaultIfEmpty()
                                select new FeedProjection()
                                {
                                    CreatedById = article.CreatedById,
                                    CreatedDate = article.CreatedDate,
                                    Description = article.Description,
                                    Id = article.Id,
                                    Name = article.Name,
                                    PictureId = ap.PictureId,
                                    FeedType = FeedType.Article
                                });


            var productPictureType = (int)ProductPictureType.Thumbnail;
            var productFeeds = (from product in _productRepository.Table
                                join productPic in _productPictureRepository.Get(x => x.PictureType == productPictureType)
                                on product.Id equals productPic.ProductId into productPics
                                from pp in productPics.DefaultIfEmpty()
                                join pr in _productPriceRepository.Get(x => x.IsCurrent)
                                on product.Id equals pr.ProductId into prices
                                from price in prices.DefaultIfEmpty()
                                select new FeedProjection()
                                {
                                    CreatedById = product.CreatedById,
                                    CreatedDate = product.CreatedDate,
                                    Description = product.Description,
                                    Id = product.Id,
                                    Name = product.Name,
                                    PictureId = pp.PictureId,
                                    FeedType = FeedType.Product,
                                    Price = price != null ? price.Price : 0
                                });

            var farmPictureType = (int)FarmPictureType.Thumbnail;
            var farmFeeds = (from farm in _farmRepository.Table
                             join farmPic in _farmPictureRepository.Get(x => x.PictureType == farmPictureType)
                             on farm.Id equals farmPic.FarmId into farmPics
                             from fp in farmPics.DefaultIfEmpty()
                             select new FeedProjection()
                             {
                                 CreatedById = farm.CreatedById,
                                 CreatedDate = farm.CreatedDate,
                                 Description = farm.Description,
                                 Id = farm.Id,
                                 Name = farm.Name,
                                 PictureId = fp.PictureId,
                                 FeedType = FeedType.Farm,
                                 Address = farm.Address
                             });

            var feeds = await articleFeeds.Union(productFeeds)
                .Union(farmFeeds)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            var filteredNumber = feeds.Count();
            var result = new BasePageList<FeedProjection>(feeds)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }
    }
}
