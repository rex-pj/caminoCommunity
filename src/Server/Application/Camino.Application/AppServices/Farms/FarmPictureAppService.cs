using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Application.Contracts.AppServices.Farms.Dtos;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Core.Contracts.Repositories.Media;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Farms;
using Camino.Core.Domains.Farms.Repositories;
using Camino.Core.Domains.Media;
using Camino.Shared.Enums;
using Camino.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.AppServices.Farms
{
    public class FarmPictureAppService : IFarmPictureAppService, IScopedDependency
    {
        private readonly IFarmPictureRepository _farmPictureRepository;
        private readonly IEntityRepository<FarmPicture> _farmPictureEntityRepository;
        private readonly IEntityRepository<Picture> _pictureEntityRepository;
        private readonly IEntityRepository<Farm> _farmEntityRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IDbContext _dbContext;
        private readonly int _deletedStatus = PictureStatuses.Deleted.GetCode();
        private readonly int _inactivedStatus = PictureStatuses.Inactived.GetCode();

        public FarmPictureAppService(IEntityRepository<FarmPicture> farmPictureEntityRepository,
           IEntityRepository<Picture> pictureEntityRepository,
           IEntityRepository<Farm> farmEntityRepository,
           IPictureRepository pictureRepository,
           IFarmPictureRepository farmPictureRepository,
           IDbContext dbContext)
        {
            _farmPictureEntityRepository = farmPictureEntityRepository;
            _pictureEntityRepository = pictureEntityRepository;
            _farmEntityRepository = farmEntityRepository;
            _pictureRepository = pictureRepository;
            _farmPictureRepository = farmPictureRepository;
            _dbContext = dbContext;
        }

        public async Task<BasePageList<FarmPictureResult>> GetAsync(FarmPictureFilter filter)
        {
            var pictureQuery = _pictureEntityRepository.Get(x => x.StatusId != PictureStatuses.Deleted.GetCode());
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

            var query = from ap in _farmPictureEntityRepository.Table
                        join p in pictureQuery
                        on ap.PictureId equals p.Id
                        join a in _farmEntityRepository.Table
                        on ap.FarmId equals a.Id
                        select new FarmPictureResult()
                        {
                            FarmId = a.Id,
                            FarmName = a.Name,
                            PictureId = p.Id,
                            PictureName = p.FileName,
                            FarmPictureTypeId = ap.PictureTypeId,
                            PictureCreatedById = p.CreatedById,
                            PictureCreatedDate = p.CreatedDate,
                            ContentType = p.MimeType
                        };

            var filteredNumber = query.Select(x => x.PictureId).Count();

            var farmPictures = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<FarmPictureResult>(farmPictures)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public async Task<IList<FarmPictureResult>> GetListByFarmIdAsync(IdRequestFilter<long> filter, int? farmPictureTypeId = null)
        {

            var farmPictures = await (from farmPic in _farmPictureEntityRepository
                                      .Get(x => x.FarmId == filter.Id && (!farmPictureTypeId.HasValue || x.PictureTypeId == farmPictureTypeId))
                                      join picture in _pictureEntityRepository
                                      .Get(x => (x.StatusId == _deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == _inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != _deletedStatus && x.StatusId != _inactivedStatus))
                                      on farmPic.PictureId equals picture.Id
                                      select new FarmPictureResult
                                      {
                                          FarmId = farmPic.FarmId,
                                          FarmPictureTypeId = farmPic.PictureTypeId,
                                          PictureId = farmPic.PictureId
                                      }).ToListAsync();
            return farmPictures;
        }

        public async Task<IList<FarmPictureResult>> GetListByFarmIdsAsync(IEnumerable<long> farmIds, IdRequestFilter<long> filter, FarmPictureTypes farmPictureType)
        {
            var farmPictureTypeId = farmPictureType.GetCode();
            var farmPictures = await (from farmPic in _farmPictureEntityRepository.Get(x => farmIds.Contains(x.FarmId) && x.PictureTypeId == farmPictureTypeId)
                                      join picture in _pictureEntityRepository
                                      .Get(x => (x.StatusId == _deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == _inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != _deletedStatus && x.StatusId != _inactivedStatus))
                                      on farmPic.PictureId equals picture.Id
                                      select new FarmPictureResult
                                      {
                                          FarmId = farmPic.FarmId,
                                          FarmPictureTypeId = farmPic.PictureTypeId,
                                          PictureId = farmPic.PictureId
                                      }).ToListAsync();
            return farmPictures;
        }

        public async Task<bool> CreateAsync(FarmPicturesModifyRequest request, bool needSaveChanges = false)
        {
            var index = 0;
            foreach (var picture in request.Pictures)
            {
                var pictureType = index == 0 ? FarmPictureTypes.Thumbnail : FarmPictureTypes.Secondary;
                await CreateAsync(request, picture, pictureType, needSaveChanges);
                index += 1;
            }

            return true;
        }

        public async Task<bool> UpdateAsync(FarmPicturesModifyRequest request, bool needSaveChanges = false)
        {
            var pictureIds = request.Pictures.Select(x => x.Id).ToList();
            await DeleteByFarmIdAsync(request.FarmId, pictureIds);

            var thumbnailType = FarmPictureTypes.Thumbnail;
            var thumbnail = await _farmPictureRepository.GetByTypeAsync(request.FarmId, thumbnailType);
            var shouldAddThumbnail = thumbnail == null;

            var index = 0;
            foreach (var picture in request.Pictures)
            {
                if (!string.IsNullOrEmpty(picture.Base64Data))
                {
                    var farmPictureType = shouldAddThumbnail && index == 0 ? thumbnailType : FarmPictureTypes.Secondary;
                    await CreateAsync(request, picture, farmPictureType, needSaveChanges);
                    shouldAddThumbnail = false;
                    index += 1;
                }
            }

            if (needSaveChanges)
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }

            return false;
        }

        private async Task<long> CreateAsync(FarmPicturesModifyRequest request, PictureRequest picture, FarmPictureTypes pictureType, bool needSaveChanges = false)
        {
            var modifiedDate = DateTime.UtcNow;
            var pictureId = await _pictureRepository.CreateAsync(new Picture
            {
                CreatedById = request.UpdatedById,
                CreatedDate = modifiedDate,
                FileName = picture.FileName,
                MimeType = picture.ContentType,
                UpdatedById = request.UpdatedById,
                UpdatedDate = modifiedDate,
                BinaryData = picture.BinaryData,
                StatusId = PictureStatuses.Pending.GetCode()
            });

            return await _farmPictureRepository.CreateAsync(new FarmPicture
            {
                FarmId = request.FarmId,
                PictureId = pictureId,
                PictureTypeId = pictureType.GetCode()
            }, needSaveChanges);
        }

        public async Task<bool> DeleteByFarmIdAsync(long farmId)
        {
            var farmPictures = _farmPictureEntityRepository.Get(x => x.FarmId == farmId);
            var pictureIds = farmPictures.Select(x => x.PictureId).ToList();
            await _farmPictureEntityRepository.DeleteAsync(farmPictures);

            await _pictureEntityRepository.DeleteAsync(x => pictureIds.Contains(x.Id));
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task DeleteByFarmIdAsync(long farmId, IList<long> pictureIds, bool needSaveChanges = false)
        {
            var deleteProductPictures = _farmPictureEntityRepository
                        .Get(x => x.FarmId == farmId && !pictureIds.Contains(x.PictureId));

            // Delete old images
            var deletePictureIds = deleteProductPictures.Select(x => x.PictureId).ToList();
            if (deletePictureIds.Any())
            {
                await _farmPictureEntityRepository.DeleteAsync(deleteProductPictures);
                await _pictureEntityRepository.DeleteAsync(x => deletePictureIds.Contains(x.Id));
            }

            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateStatusByFarmIdAsync(long farmId, long updatedById, PictureStatuses pictureStatus)
        {
            var existingPictures = (from farmPicture in _farmPictureEntityRepository.Get(x => x.FarmId == farmId)
                                    join picture in _pictureEntityRepository.Table
                                    on farmPicture.PictureId equals picture.Id
                                    select picture);

            foreach (var picture in existingPictures)
            {
                picture.StatusId = pictureStatus.GetCode();
                picture.UpdatedById = updatedById;
                picture.UpdatedDate = DateTime.UtcNow;
            }

            await _pictureEntityRepository.UpdateAsync(existingPictures);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
