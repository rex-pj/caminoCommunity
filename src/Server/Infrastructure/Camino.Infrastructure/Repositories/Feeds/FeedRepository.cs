using Camino.Core.Contracts.Data;
using Camino.Shared.Enums;
using Camino.Core.Contracts.Repositories.Feeds;
using Camino.Shared.Results.Feed;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domain.Articles;
using Camino.Core.Domain.Farms;
using Camino.Core.Domain.Products;
using Camino.Core.Domain.Media;
using Camino.Core.Utils;
using Camino.Core.Domain.Identifiers;
using System.Collections.Generic;
using Camino.Shared.Results.General;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Infrastructure.Repositories.Feeds
{
    public class FeedRepository : IFeedRepository
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<ArticlePicture> _articlePictureRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<FarmPicture> _farmPictureRepository;
        private readonly IRepository<ProductPrice> _productPriceRepository;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<User> _userRepository;
        private readonly int _articleDeletedStatus;
        private readonly int _articleInactivedStatus;
        private readonly int _productDeletedStatus;
        private readonly int _productInactivedStatus;
        private readonly int _farmDeletedStatus;
        private readonly int _farmInactivedStatus;
        private readonly int _userActivedStatus;
        private readonly int _pictureDeletedStatus;
        private readonly int _pictureInactivedStatus;

        public FeedRepository(IRepository<ArticlePicture> articlePictureRepository,
            IRepository<ProductPicture> productPictureRepository, IRepository<FarmPicture> farmPictureRepository,
            IRepository<UserPhoto> userPhotoRepository,
            IRepository<Picture> pictureRepository, IServiceScopeFactory serviceScopeFactory)
        {
            _articleRepository = CreateServiceScope<IRepository<Article>>(serviceScopeFactory);
            _productRepository = CreateServiceScope<IRepository<Product>>(serviceScopeFactory);
            _farmRepository = CreateServiceScope<IRepository<Farm>>(serviceScopeFactory);
            _userRepository = CreateServiceScope<IRepository<User>>(serviceScopeFactory);
            _productPriceRepository = CreateServiceScope<IRepository<ProductPrice>>(serviceScopeFactory);
            _farmPictureRepository = farmPictureRepository;
            _articlePictureRepository = articlePictureRepository;
            _productPictureRepository = productPictureRepository;
            _userPhotoRepository = userPhotoRepository;
            _pictureRepository = pictureRepository;

            _articleDeletedStatus = ArticleStatus.Deleted.GetCode();
            _articleInactivedStatus = ArticleStatus.Inactived.GetCode();
            _productDeletedStatus = ProductStatus.Deleted.GetCode();
            _productInactivedStatus = ProductStatus.Inactived.GetCode();
            _farmDeletedStatus = FarmStatus.Deleted.GetCode();
            _farmInactivedStatus = FarmStatus.Inactived.GetCode();
            _userActivedStatus = UserStatus.Actived.GetCode();
            _pictureDeletedStatus = PictureStatus.Deleted.GetCode();
            _pictureInactivedStatus = PictureStatus.Inactived.GetCode();
        }

        private TService CreateServiceScope<TService>(IServiceScopeFactory serviceScopeFactory)
        {
            return serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TService>();
        }

        public async Task<BasePageList<FeedResult>> GetAsync(FeedFilter filter)
        {
            var articleQuery = _articleRepository.Get(x => (x.StatusId == _articleDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _articleInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _articleDeletedStatus && x.StatusId != _articleInactivedStatus));

            var productQuery = _productRepository.Get(x => (x.StatusId == _productDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _productInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _productDeletedStatus && x.StatusId != _productInactivedStatus));

            var farmQuery = _farmRepository.Get(x => (x.StatusId == _farmDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _farmInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _farmDeletedStatus && x.StatusId != _farmInactivedStatus));

            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();

                articleQuery = articleQuery.Where(x => x.Name.ToLower().Contains(filter.Search) || x.Description.Contains(filter.Search));
                productQuery = productQuery.Where(x => x.Name.ToLower().Contains(filter.Search) || x.Description.Contains(filter.Search));
                farmQuery = farmQuery.Where(x => x.Name.ToLower().Contains(filter.Search) || x.Description.Contains(filter.Search));
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

            var articleFeeds = (from article in articleQuery
                                select new FeedResult()
                                {
                                    CreatedById = article.CreatedById,
                                    CreatedDate = article.CreatedDate,
                                    Description = string.IsNullOrEmpty(article.Description) ? article.Content : article.Description,
                                    Id = article.Id.ToString(),
                                    Name = article.Name,
                                    FeedType = FeedType.Article,
                                });

            var productFeeds = (from product in productQuery
                                join pr in _productPriceRepository.Get(x => x.IsCurrent)
                                on product.Id equals pr.ProductId into prices
                                from price in prices.DefaultIfEmpty()
                                select new FeedResult
                                {
                                    CreatedById = product.CreatedById,
                                    CreatedDate = product.CreatedDate,
                                    Description = product.Description,
                                    Id = product.Id.ToString(),
                                    Name = product.Name,
                                    FeedType = FeedType.Product,
                                    Price = price != null ? price.Price : 0,
                                });

            var farmFeeds = (from farm in farmQuery
                             select new FeedResult()
                             {
                                 CreatedById = farm.CreatedById,
                                 CreatedDate = farm.CreatedDate,
                                 Description = farm.Description,
                                 Id = farm.Id.ToString(),
                                 Name = farm.Name,
                                 FeedType = FeedType.Farm,
                                 Address = farm.Address,
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

            var result = new BasePageList<FeedResult>(feeds)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public async Task<SearchInGroupResult> GetInGroupAsync(FeedFilter filter)
        {
            var articleQuery = _articleRepository.Get(x => (x.StatusId == _articleDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _articleInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _articleDeletedStatus && x.StatusId != _articleInactivedStatus));

            var productQuery = _productRepository.Get(x => (x.StatusId == _productDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _productInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _productDeletedStatus && x.StatusId != _productInactivedStatus));

            var farmQuery = _farmRepository.Get(x => (x.StatusId == _farmDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _farmInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _farmDeletedStatus && x.StatusId != _farmInactivedStatus));

            var userQuery = _userRepository.Get(x => x.IsEmailConfirmed && (filter.CanGetInactived || x.StatusId == _userActivedStatus));
            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();

                articleQuery = articleQuery.Where(x => x.Name.ToLower().Contains(filter.Search) || x.Description.ToLower().Contains(filter.Search));
                productQuery = productQuery.Where(x => x.Name.ToLower().Contains(filter.Search) || x.Description.ToLower().Contains(filter.Search));
                farmQuery = farmQuery.Where(x => x.Name.ToLower().Contains(filter.Search) || x.Description.ToLower().Contains(filter.Search));
                userQuery = userQuery.Where(x => x.Lastname.ToLower().Contains(filter.Search)
                || x.Firstname.ToLower().Contains(filter.Search)
                || (x.Lastname + " " + x.Firstname).ToLower().Contains(filter.Search)
                || (x.Firstname + " " + x.Lastname).ToLower().Contains(filter.Search));
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

            var articleFeeds = (from article in articleQuery
                                select new FeedResult()
                                {
                                    CreatedById = article.CreatedById,
                                    CreatedDate = article.CreatedDate,
                                    Description = string.IsNullOrEmpty(article.Description) ? article.Content : article.Description,
                                    Id = article.Id.ToString(),
                                    Name = article.Name,
                                    FeedType = FeedType.Article,
                                });

            var productFeeds = (from product in productQuery
                                join pr in _productPriceRepository.Get(x => x.IsCurrent)
                                on product.Id equals pr.ProductId into prices
                                from price in prices.DefaultIfEmpty()
                                select new FeedResult
                                {
                                    CreatedById = product.CreatedById,
                                    CreatedDate = product.CreatedDate,
                                    Description = product.Description,
                                    Id = product.Id.ToString(),
                                    Name = product.Name,
                                    FeedType = FeedType.Product,
                                    Price = price != null ? price.Price : 0,
                                });

            var farmFeeds = (from farm in farmQuery
                             select new FeedResult()
                             {
                                 CreatedById = farm.CreatedById,
                                 CreatedDate = farm.CreatedDate,
                                 Description = farm.Description,
                                 Id = farm.Id.ToString(),
                                 Name = farm.Name,
                                 FeedType = FeedType.Farm,
                                 Address = farm.Address,
                             });

            var userFeeds = (from user in userQuery
                             select new FeedResult()
                             {
                                 CreatedById = user.CreatedById,
                                 CreatedDate = user.CreatedDate,
                                 Description = user.UserInfo.Description,
                                 Id = user.Id.ToString(),
                                 Name = user.Lastname + " " + user.Firstname,
                                 FeedType = FeedType.User,
                                 Address = user.UserInfo.Address,
                             });

            var countingTasks = new List<Task<CountingTask>>();
            countingTasks.Add(Task.Run(async () =>
            {
                return new CountingTask
                {
                    TaskName = FeedType.Article.ToString(),
                    TotalResult = await articleFeeds.CountAsync()
                };
            }));
            countingTasks.Add(Task.Run(async () =>
            {
                return new CountingTask
                {
                    TaskName = FeedType.Product.ToString(),
                    TotalResult = await productFeeds.CountAsync()
                };
            }));
            countingTasks.Add(Task.Run(async () =>
            {
                return new CountingTask
                {
                    TaskName = FeedType.Farm.ToString(),
                    TotalResult = await farmFeeds.CountAsync()
                };
            }));
            countingTasks.Add(Task.Run(async () =>
            {
                return new CountingTask
                {
                    TaskName = FeedType.User.ToString(),
                    TotalResult = await userFeeds.CountAsync()
                };
            }));

            await Task.WhenAll(countingTasks);

            var totalUser = countingTasks.FirstOrDefault(x => x.Result.TaskName == FeedType.User.ToString()).Result.TotalResult;
            var totalFarm = countingTasks.FirstOrDefault(x => x.Result.TaskName == FeedType.Farm.ToString()).Result.TotalResult;
            var totalArticle = countingTasks.FirstOrDefault(x => x.Result.TaskName == FeedType.Article.ToString()).Result.TotalResult;
            var totalProduct = countingTasks.FirstOrDefault(x => x.Result.TaskName == FeedType.Product.ToString()).Result.TotalResult;

            var feedTasks = new List<Task<List<FeedResult>>>();
            feedTasks.Add(Task.Run(async () =>
            {
                int page = filter.Page;
                int pageSize = filter.PageSize;
                return await userFeeds
                  .OrderByDescending(x => x.CreatedDate)
                  .Skip(pageSize * (page - 1))
                  .Take(pageSize)
                  .ToListAsync();
            }));

            feedTasks.Add(Task.Run(async () =>
            {
                int page = filter.Page;
                int pageSize = filter.PageSize;
                return await articleFeeds
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
            }));

            feedTasks.Add(Task.Run(async () =>
            {
                int page = filter.Page;
                int pageSize = filter.PageSize;
                return await productFeeds
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
            }));

            feedTasks.Add(Task.Run(async () =>
            {
                int page = filter.Page;
                int pageSize = filter.PageSize;
                return await farmFeeds
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
            }));

            await Task.WhenAll(feedTasks);

            var articles = feedTasks.SelectMany(x => x.Result.Where(r => r.FeedType == FeedType.Article)).ToList();
            var products = feedTasks.SelectMany(x => x.Result.Where(r => r.FeedType == FeedType.Product)).ToList();
            var farms = feedTasks.SelectMany(x => x.Result.Where(r => r.FeedType == FeedType.Farm)).ToList();
            var users = feedTasks.SelectMany(x => x.Result.Where(r => r.FeedType == FeedType.User)).ToList();

            var result = new SearchInGroupResult
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

            return result;
        }
    }
}
