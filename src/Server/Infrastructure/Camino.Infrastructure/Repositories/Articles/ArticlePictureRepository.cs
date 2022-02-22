using System.Threading.Tasks;
using System.Linq;
using LinqToDB;
using Camino.Shared.Requests.Filters;
using System;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Articles;
using Camino.Core.Domain.Articles;
using Camino.Core.Domain.Media;
using System.Collections.Generic;
using Camino.Shared.Requests.Articles;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;
using Camino.Infrastructure.Linq2Db.Extensions;

namespace Camino.Infrastructure.Repositories.Articles
{
    public class ArticlePictureRepository : IArticlePictureRepository, IScopedDependency
    {
        private readonly IEntityRepository<ArticlePicture> _articlePictureRepository;
        private readonly IEntityRepository<Picture> _pictureRepository;
        private readonly IEntityRepository<Article> _articleRepository;

        public ArticlePictureRepository(IEntityRepository<ArticlePicture> articlePictureRepository, IEntityRepository<Picture> pictureRepository,
            IEntityRepository<Article> articleRepository)
        {
            _articlePictureRepository = articlePictureRepository;
            _pictureRepository = pictureRepository;
            _articleRepository = articleRepository;
        }

        public async Task<BasePageList<ArticlePictureResult>> GetAsync(ArticlePictureFilter filter)
        {
            var pictureQuery = _pictureRepository.Get(x => x.StatusId != PictureStatus.Pending.GetCode());
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                var search = filter.Keyword.ToLower();
                pictureQuery = pictureQuery.Where(pic => pic.Title.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (!string.IsNullOrEmpty(filter.MimeType))
            {
                var mimeType = filter.MimeType.ToLower();
                pictureQuery = pictureQuery.Where(x => x.MimeType.Contains(mimeType));
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTimeOffset.UtcNow);
            }

            var query = from ap in _articlePictureRepository.Table
                        join p in pictureQuery
                        on ap.PictureId equals p.Id
                        join a in _articleRepository.Table
                        on ap.ArticleId equals a.Id
                        select new ArticlePictureResult()
                        {
                            ArticleId = a.Id,
                            ArticleName = a.Name,
                            PictureId = p.Id,
                            PictureName = p.FileName,
                            ArticlePictureTypeId = ap.PictureTypeId,
                            PictureCreatedById = p.CreatedById,
                            PictureCreatedDate = p.CreatedDate,
                            ContentType = p.MimeType
                        };

            var filteredNumber = query.Select(x => x.PictureId).Count();

            var articlePictures = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ArticlePictureResult>(articlePictures)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public async Task<ArticlePictureResult> GetArticlePictureByArticleIdAsync(IdRequestFilter<long> filter)
        {
            var deletedStatus = PictureStatus.Deleted.GetCode();
            var inactivedStatus = PictureStatus.Inactived.GetCode();
            var articlePicture = await (from articlePic in _articlePictureRepository.Get(x => x.ArticleId == filter.Id)
                                        join picture in _pictureRepository
                                        .Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
                                        on articlePic.PictureId equals picture.Id
                                        select new ArticlePictureResult
                                        {
                                            ArticleId = articlePic.ArticleId,
                                            ArticlePictureTypeId = articlePic.PictureTypeId,
                                            PictureId = articlePic.PictureId
                                        }).FirstOrDefaultAsync();
            return articlePicture;
        }

        public async Task<IList<ArticlePictureResult>> GetArticlePicturesByArticleIdsAsync(IEnumerable<long> articleIds, IdRequestFilter<long> filter)
        {
            return await GetArticlePicturesByArticleIdsAsync(articleIds, filter, null);
        }

        public async Task<IList<ArticlePictureResult>> GetArticlePicturesByArticleIdsAsync(IEnumerable<long> articleIds, IdRequestFilter<long> filter, ArticlePictureType? articlePictureType)
        {
            var deletedStatus = PictureStatus.Deleted.GetCode();
            var inactivedStatus = PictureStatus.Inactived.GetCode();
            var articlePictureTypeId = articlePictureType.HasValue ? articlePictureType.Value.GetCode() : 0;
            var articlePictures = await (from articlePic in _articlePictureRepository.Get(x => articleIds.Contains(x.ArticleId))
                                         join picture in _pictureRepository.Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
                                         on articlePic.PictureId equals picture.Id
                                         where articlePictureTypeId == 0 || articlePic.PictureTypeId == articlePictureTypeId
                                         select new ArticlePictureResult
                                         {
                                             ArticleId = articlePic.ArticleId,
                                             ArticlePictureTypeId = articlePic.PictureTypeId,
                                             PictureId = articlePic.PictureId
                                         }).ToListAsync();
            return articlePictures;
        }

        public async Task<long> CreateAsync(ArticlePictureModifyRequest request)
        {
            var base64Data = ImageUtil.EncodeJavascriptBase64(request.Picture.Base64Data);
            var pictureData = Convert.FromBase64String(base64Data);
            var pictureId = await _pictureRepository.AddAsync<long>(new Picture
            {
                CreatedById = request.UpdatedById,
                CreatedDate = request.CreatedDate,
                FileName = request.Picture.FileName,
                MimeType = request.Picture.ContentType,
                UpdatedById = request.UpdatedById,
                UpdatedDate = request.UpdatedDate,
                BinaryData = pictureData,
                StatusId = PictureStatus.Pending.GetCode()
            });

            var id = await _articlePictureRepository.AddAsync<long>(new ArticlePicture()
            {
                ArticleId = request.ArticleId,
                PictureId = pictureId,
                PictureTypeId = (int)ArticlePictureType.Thumbnail
            });

            return id;
        }

        public async Task<bool> UpdateAsync(ArticlePictureModifyRequest request)
        {
            var pictureTypeId = (int)ArticlePictureType.Thumbnail;
            var shouldRemovePicture = request.Picture.Id == 0 && string.IsNullOrEmpty(request.Picture.Base64Data);
            var shouldUpdatePicture = request.Picture.Id == 0 && !string.IsNullOrEmpty(request.Picture.Base64Data);

            // Remove Old thumbnail
            if (shouldRemovePicture || shouldUpdatePicture)
            {
                var articlePictures = _articlePictureRepository
                    .Get(x => x.ArticleId == request.ArticleId && x.PictureTypeId == pictureTypeId);
                if (articlePictures.Any())
                {
                    var pictureIds = articlePictures.Select(x => x.PictureId).ToList();
                    await _articlePictureRepository.DeleteAsync(articlePictures);

                    await _pictureRepository.DeleteAsync(x => pictureIds.Contains(x.Id));
                }
            }

            if (shouldUpdatePicture)
            {
                var base64Data = ImageUtil.EncodeJavascriptBase64(request.Picture.Base64Data);
                var pictureData = Convert.FromBase64String(base64Data);
                var pictureId = await _pictureRepository.AddAsync<long>(new Picture()
                {
                    CreatedById = request.UpdatedById,
                    CreatedDate = request.CreatedDate,
                    FileName = request.Picture.FileName,
                    MimeType = request.Picture.ContentType,
                    UpdatedById = request.UpdatedById,
                    UpdatedDate = request.UpdatedDate,
                    BinaryData = pictureData
                });

                await _articlePictureRepository.AddAsync(new ArticlePicture()
                {
                    ArticleId = request.ArticleId,
                    PictureId = pictureId,
                    PictureTypeId = pictureTypeId
                });
            }

            return true;
        }

        public async Task<bool> DeleteByArticleIdAsync(long articleId)
        {
            var articlePictures = _articlePictureRepository.Get(x => x.ArticleId == articleId);
            var pictureIds = articlePictures.Select(x => x.PictureId).ToList();
            await _articlePictureRepository.DeleteAsync(articlePictures);

            await _pictureRepository.DeleteAsync(x => pictureIds.Contains(x.Id));

            return true;
        }

        public async Task<bool> UpdateStatusByArticleIdAsync(ArticlePictureModifyRequest request, PictureStatus pictureStatus)
        {
            await (from articlePicture in _articlePictureRepository.Get(x => x.ArticleId == request.ArticleId)
                   join picture in _pictureRepository.Table
                   on articlePicture.PictureId equals picture.Id
                   select picture)
                .SetEntry(x => x.StatusId, pictureStatus.GetCode())
                .SetEntry(x => x.UpdatedById, request.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }
    }
}
