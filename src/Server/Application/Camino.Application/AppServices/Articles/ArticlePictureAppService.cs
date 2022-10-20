using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Articles;
using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Application.Contracts.AppServices.Articles.Dtos.Dtos;
using Camino.Core.Contracts.Repositories.Media;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domains;
using Camino.Core.Domains.Articles;
using Camino.Core.Domains.Articles.DomainServices;
using Camino.Core.Domains.Articles.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Media;
using Camino.Shared.Enums;
using Camino.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Camino.Application.AppServices.Articles
{
    public class ArticlePictureAppService : IArticlePictureAppService, IScopedDependency
    {
        private readonly IEntityRepository<ArticlePicture> _articlePictureEntityRepository;
        private readonly IEntityRepository<Picture> _pictureEntityRepository;
        private readonly IEntityRepository<Article> _articleEntityRepository;
        private readonly IArticlePictureRepository _articlePictureRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IUserRepository _userRepository;
        private readonly IArticlePictureDomainService _articlePictureDomainService;
        private readonly int _pendingStatus = PictureStatuses.Pending.GetCode();
        private readonly int _inactivedStatus = PictureStatuses.Inactived.GetCode();
        private readonly int _deletedStatus = PictureStatuses.Deleted.GetCode();

        public ArticlePictureAppService(IEntityRepository<ArticlePicture> articlePictureEntityRepository,
            IEntityRepository<Picture> pictureEntityRepository,
            IEntityRepository<Article> articleEntityRepository,
            IArticlePictureRepository articlePictureRepository,
            IPictureRepository pictureRepository,
            IUserRepository userRepository,
            IArticlePictureDomainService articlePictureDomainService)
        {
            _articlePictureEntityRepository = articlePictureEntityRepository;
            _pictureEntityRepository = pictureEntityRepository;
            _articleEntityRepository = articleEntityRepository;
            _articlePictureRepository = articlePictureRepository;
            _pictureRepository = pictureRepository;
            _userRepository = userRepository;
            _articlePictureDomainService = articlePictureDomainService;
        }

        public async Task<BasePageList<ArticlePictureResult>> GetAsync(ArticlePictureFilter filter)
        {
            var pictureQuery = _pictureEntityRepository.Get(x => x.StatusId != _pendingStatus);
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
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var query = from ap in _articlePictureEntityRepository.Table
                        join p in pictureQuery
                        on ap.PictureId equals p.Id
                        join a in _articleEntityRepository.Table
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

            await PopulateDetailsAsync(articlePictures);
            var result = new BasePageList<ArticlePictureResult>(articlePictures)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        private async Task PopulateDetailsAsync(IList<ArticlePictureResult> articlePictures)
        {
            var createdByIds = articlePictures.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);
            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);

            foreach (var articlePicture in articlePictures)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == articlePicture.PictureCreatedById);
                articlePicture.PictureCreatedBy = createdBy.DisplayName;
            }
        }

        public async Task<ArticlePictureResult> GetByArticleIdAsync(IdRequestFilter<long> filter)
        {
            var deletedStatus = PictureStatuses.Deleted.GetCode();
            var inactivedStatus = PictureStatuses.Inactived.GetCode();
            var articlePicture = await (from articlePic in _articlePictureEntityRepository.Get(x => x.ArticleId == filter.Id)
                                        join picture in _pictureEntityRepository
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

        public async Task<IList<ArticlePictureResult>> GetListByArticleIdsAsync(IEnumerable<long> articleIds, IdRequestFilter<long> filter)
        {
            return await GetListByArticleIdsAsync(articleIds, filter, null);
        }

        public async Task<IList<ArticlePictureResult>> GetListByArticleIdsAsync(IEnumerable<long> articleIds, IdRequestFilter<long> filter, ArticlePictureTypes? articlePictureType)
        {
            var articlePictureTypeId = articlePictureType.HasValue ? articlePictureType.Value.GetCode() : 0;
            var articlePictures = await (from articlePic in _articlePictureEntityRepository.Get(x => articleIds.Contains(x.ArticleId))
                                         join picture in _pictureEntityRepository.Get(x => (x.StatusId == _deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == _inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != _deletedStatus && x.StatusId != _inactivedStatus))
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
            return await CreateAsync(request, ArticlePictureTypes.Thumbnail, true);
        }

        public async Task<bool> UpdateAsync(ArticlePictureModifyRequest request)
        {
            var shouldRemoveOldPicture = request.Picture.Id == 0;
            if (shouldRemoveOldPicture)
            {
                await _articlePictureDomainService.DeleteByArticleIdAsync(request.ArticleId);
            }

            var shouldUpdatePicture = request.Picture.Id == 0 && !string.IsNullOrEmpty(request.Picture.Base64Data);
            if (shouldUpdatePicture)
            {
                var articlePictureId = await CreateAsync(request, ArticlePictureTypes.Thumbnail, true);
                return articlePictureId > 0;
            }

            return false;
        }

        private async Task<long> CreateAsync(ArticlePictureModifyRequest request, ArticlePictureTypes pictureType, bool needSaveChanges = false)
        {
            var modifiedDate = DateTime.UtcNow;
            var base64Data = ImageUtils.EncodeJavascriptBase64(request.Picture.Base64Data);
            var pictureData = Convert.FromBase64String(base64Data);
            var pictureId = await _pictureRepository.CreateAsync(new Picture
            {
                CreatedById = request.UpdatedById,
                CreatedDate = modifiedDate,
                FileName = request.Picture.FileName,
                MimeType = request.Picture.ContentType,
                UpdatedById = request.UpdatedById,
                UpdatedDate = modifiedDate,
                BinaryData = pictureData,
                StatusId = PictureStatuses.Pending.GetCode()
            });

            return await _articlePictureRepository.CreateAsync(new ArticlePicture
            {
                ArticleId = request.ArticleId,
                PictureId = pictureId,
                PictureTypeId = pictureType.GetCode()
            }, needSaveChanges);
        }
    }
}
