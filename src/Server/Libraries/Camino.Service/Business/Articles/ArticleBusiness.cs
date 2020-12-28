using AutoMapper;
using Camino.Service.Projections.Filters;
using Camino.Data.Contracts;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Articles.Contracts;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Data.Enums;
using Camino.Core.Utils;
using Camino.Service.Projections.Article;
using System.Collections.Generic;
using Camino.Service.Projections.Media;

namespace Camino.Service.Business.Articles
{
    public class ArticleBusiness : IArticleBusiness
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<ArticlePicture> _articlePictureRepository;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ArticleBusiness(IMapper mapper, IRepository<Article> articleRepository,
            IRepository<ArticleCategory> articleCategoryRepository, IRepository<User> userRepository,
            IRepository<Picture> pictureRepository, IRepository<ArticlePicture> articlePictureRepository,
            IRepository<UserPhoto> userPhotoRepository)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _userRepository = userRepository;
            _pictureRepository = pictureRepository;
            _articlePictureRepository = articlePictureRepository;
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<ArticleProjection> FindAsync(long id)
        {
            var exist = await (from article in _articleRepository.Table
                               join category in _articleCategoryRepository.Table
                               on article.ArticleCategoryId equals category.Id
                               where article.Id == id
                               select new ArticleProjection
                               {
                                   CreatedDate = article.CreatedDate,
                                   CreatedById = article.CreatedById,
                                   Id = article.Id,
                                   Name = article.Name,
                                   UpdatedById = article.UpdatedById,
                                   UpdatedDate = article.UpdatedDate,
                                   ArticleCategoryName = category.Name,
                                   ArticleCategoryId = article.ArticleCategoryId
                               }).FirstOrDefaultAsync();

            if (exist == null)
            {
                return null;
            }

            return exist;
        }

        public async Task<ArticleProjection> FindDetailAsync(long id)
        {
            var pictureTypeId = (int)ArticlePictureType.Thumbnail;
            var exist = await (from article in _articleRepository.Table
                               join category in _articleCategoryRepository.Table
                               on article.ArticleCategoryId equals category.Id
                               join articlePic in _articlePictureRepository.Get(x => x.PictureType == pictureTypeId)
                               on article.Id equals articlePic.ArticleId into articlePics
                               from picture in articlePics.DefaultIfEmpty()
                               where article.Id == id
                               select new ArticleProjection
                               {
                                   Description = article.Description,
                                   CreatedDate = article.CreatedDate,
                                   CreatedById = article.CreatedById,
                                   Id = article.Id,
                                   Name = article.Name,
                                   UpdatedById = article.UpdatedById,
                                   UpdatedDate = article.UpdatedDate,
                                   ArticleCategoryName = category.Name,
                                   ArticleCategoryId = article.ArticleCategoryId,
                                   Content = article.Content,
                                   Thumbnail = new PictureRequestProjection
                                   {
                                       Id = picture.PictureId
                                   }
                               }).FirstOrDefaultAsync();

            if (exist == null)
            {
                return null;
            }

            var createdByUserName = await _userRepository.Get(x => x.Id == exist.CreatedById).Select(x => x.DisplayName).FirstOrDefaultAsync();
            exist.CreatedBy = createdByUserName;

            return exist;
        }

        public ArticleProjection FindByName(string name)
        {
            var exist = _articleRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var article = _mapper.Map<ArticleProjection>(exist);

            return article;
        }

        public async Task<BasePageList<ArticleProjection>> GetAsync(ArticleFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var articleQuery = _articleRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                articleQuery = articleQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            var content = filter.Content != null ? filter.Content.ToLower() : "";
            if (!string.IsNullOrEmpty(content))
            {
                articleQuery = articleQuery.Where(user => user.Content.ToLower().Contains(content));
            }

            if (filter.CreatedById.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.CategoryId.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.ArticleCategoryId == filter.CategoryId);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var filteredNumber = articleQuery.Select(x => x.Id).Count();

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var query = from ar in articleQuery
                        join pic in _articlePictureRepository.Table
                        on ar.Id equals pic.ArticleId into pics
                        from picture in pics.DefaultIfEmpty()
                        join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                        on ar.CreatedById equals pho.UserId into photos
                        from photo in photos.DefaultIfEmpty()
                        select new ArticleProjection
                        {
                            Id = ar.Id,
                            Name = ar.Name,
                            CreatedById = ar.CreatedById,
                            CreatedDate = ar.CreatedDate,
                            Description = ar.Description,
                            UpdatedById = ar.UpdatedById,
                            UpdatedDate = ar.UpdatedDate,
                            Thumbnail = new PictureRequestProjection
                            {
                                Id = picture.PictureId
                            },
                            Content = ar.Content,
                            CreatedByPhotoCode = photo.Code
                        };

            var articles = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var createdByIds = articles.Select(x => x.CreatedById).ToArray();
            var updatedByIds = articles.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();

            foreach (var article in articles)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == article.CreatedById);
                article.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == article.UpdatedById);
                article.UpdatedBy = updatedBy.DisplayName;
                article.Description = string.IsNullOrEmpty(article.Description)
                    ? HtmlUtil.TrimHtml(article.Content)
                    : article.Description;
            }

            var result = new BasePageList<ArticleProjection>(articles)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<int> CreateAsync(ArticleProjection article)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var newArticle = new Article()
            {
                ArticleCategoryId = article.ArticleCategoryId,
                Content = article.Content,
                CreatedById = article.CreatedById,
                UpdatedById = article.UpdatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Description = article.Description,
                Name = article.Name
            };

            var id = await _articleRepository.AddWithInt32EntityAsync(newArticle);

            var thumbnail = ImageUtil.EncodeJavascriptBase64(article.Thumbnail.Base64Data);
            if (!string.IsNullOrEmpty(thumbnail))
            {
                var pictureData = Convert.FromBase64String(thumbnail);
                var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                {
                    CreatedById = article.UpdatedById,
                    CreatedDate = modifiedDate,
                    FileName = article.Thumbnail.FileName,
                    MimeType = article.Thumbnail.ContentType,
                    UpdatedById = article.UpdatedById,
                    UpdatedDate = modifiedDate,
                    BinaryData = pictureData
                });

                _articlePictureRepository.Add(new ArticlePicture()
                {
                    ArticleId = id,
                    PictureId = pictureId,
                    PictureType = (int)ArticlePictureType.Thumbnail
                });
            }

            return id;
        }

        public async Task<ArticleProjection> UpdateAsync(ArticleProjection article)
        {
            var updatedDate = DateTimeOffset.UtcNow;
            var exist = _articleRepository.FirstOrDefault(x => x.Id == article.Id);
            exist.Name = article.Name;
            exist.ArticleCategoryId = article.ArticleCategoryId;
            exist.UpdatedById = article.UpdatedById;
            exist.UpdatedDate = updatedDate;
            exist.Content = article.Content;

            var thumbnail = article.Thumbnail;

            var thumbnailType = (int)ArticlePictureType.Thumbnail;
            var shouldRemoveThumbnail = thumbnail.Id == 0 && string.IsNullOrEmpty(thumbnail.Base64Data);
            var shouldUpdateThumbnail = thumbnail.Id == 0 && !string.IsNullOrEmpty(thumbnail.Base64Data);

            // Remove Old thumbnail
            if (shouldRemoveThumbnail || shouldUpdateThumbnail)
            {
                var articleThumbnails = _articlePictureRepository
                    .Get(x => x.ArticleId == article.Id && x.PictureType == thumbnailType)
                    .AsEnumerable();

                if (articleThumbnails.Any())
                {
                    var pictureIds = articleThumbnails.Select(x => x.PictureId).ToList();
                    await _articlePictureRepository.DeleteAsync(articleThumbnails.AsQueryable());

                    var currentThumbnails = _pictureRepository.Get(x => pictureIds.Contains(x.Id));
                    await _pictureRepository.DeleteAsync(currentThumbnails);
                }
            }

            if (shouldUpdateThumbnail)
            {
                var base64Data = ImageUtil.EncodeJavascriptBase64(article.Thumbnail.Base64Data);
                var pictureData = Convert.FromBase64String(base64Data);
                var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                {
                    CreatedById = article.UpdatedById,
                    CreatedDate = updatedDate,
                    FileName = article.Thumbnail.FileName,
                    MimeType = article.Thumbnail.ContentType,
                    UpdatedById = article.UpdatedById,
                    UpdatedDate = updatedDate,
                    BinaryData = pictureData
                });

                _articlePictureRepository.Add(new ArticlePicture()
                {
                    ArticleId = exist.Id,
                    PictureId = pictureId,
                    PictureType = thumbnailType
                });
            }

            _articleRepository.Update(exist);
            return article;
        }

        public async Task<IList<ArticleProjection>> GetRelevantsAsync(long id, ArticleFilter filter)
        {
            var exist = (from ar in _articleRepository.Get(x => x.Id == id)
                         select new ArticleProjection
                         {
                             Id = ar.Id,
                             CreatedById = ar.CreatedById,
                             UpdatedById = ar.UpdatedById,
                             ArticleCategoryId = ar.ArticleCategoryId
                         }).FirstOrDefault();

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var relevantArticleQuery = (from ar in _articleRepository.Get(x => x.Id != exist.Id)
                                        join pic in _articlePictureRepository.Table
                                        on ar.Id equals pic.ArticleId into pics
                                        from picture in pics.DefaultIfEmpty()
                                        join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                                        on ar.CreatedById equals pho.UserId into photos
                                        from photo in photos.DefaultIfEmpty()
                                        where ar.CreatedById == exist.CreatedById
                                        || ar.ArticleCategoryId == exist.ArticleCategoryId
                                        || ar.UpdatedById == exist.UpdatedById
                                        select new ArticleProjection
                                        {
                                            Id = ar.Id,
                                            Name = ar.Name,
                                            CreatedById = ar.CreatedById,
                                            CreatedDate = ar.CreatedDate,
                                            Description = ar.Description,
                                            UpdatedById = ar.UpdatedById,
                                            UpdatedDate = ar.UpdatedDate,
                                            Thumbnail = new PictureRequestProjection()
                                            {
                                                Id = picture.PictureId
                                            },
                                            Content = ar.Content,
                                            CreatedByPhotoCode = photo.Code
                                        });

            var relevantArticles = await relevantArticleQuery
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var createdByIds = relevantArticles.Select(x => x.CreatedById).ToArray();
            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();

            foreach (var article in relevantArticles)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == article.CreatedById);
                article.CreatedBy = createdBy.DisplayName;
            }

            return relevantArticles;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var articlePictures = _articlePictureRepository.Get(x => x.ArticleId == id);
            var pictureIds = articlePictures.Select(x => x.PictureId).ToList();
            await _articlePictureRepository.DeleteAsync(articlePictures);

            var pictures = _pictureRepository.Get(x => pictureIds.Contains(x.Id));
            await _pictureRepository.DeleteAsync(pictures);

            var existArticle = await _articleRepository.FirstOrDefaultAsync(x => x.Id == id);
            await _articleRepository.DeleteAsync(existArticle);

            return true;
        }
    }
}
