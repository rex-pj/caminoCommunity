using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Core.Domain.Farms;
using Camino.Core.Domain.Media;
using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Shared.Requests.Farms;
using LinqToDB.Tools;

namespace Camino.Infrastructure.Repositories.Farms
{
    public class FarmPictureRepository : IFarmPictureRepository
    {
        private readonly IRepository<FarmPicture> _farmPictureRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Farm> _farmRepository;

        public FarmPictureRepository(IRepository<FarmPicture> farmPictureRepository, IRepository<Picture> pictureRepository,
           IRepository<Farm> farmRepository)
        {
            _farmPictureRepository = farmPictureRepository;
            _pictureRepository = pictureRepository;
            _farmRepository = farmRepository;
        }

        public async Task<BasePageList<FarmPictureResult>> GetAsync(FarmPictureFilter filter)
        {
            var pictureQuery = _pictureRepository.Get(x => x.StatusId != PictureStatus.Deleted.GetCode());
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var search = filter.Search.ToLower();
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

            var query = from ap in _farmPictureRepository.Table
                        join p in pictureQuery
                        on ap.PictureId equals p.Id
                        join a in _farmRepository.Table
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

        public async Task<IList<FarmPictureResult>> GetFarmPicturesByFarmIdAsync(IdRequestFilter<long> filter, int? farmPictureTypeId = null)
        {
            var deletedStatus = PictureStatus.Deleted.GetCode();
            var inactivedStatus = PictureStatus.Inactived.GetCode();
            var farmPictures = await (from farmPic in _farmPictureRepository
                                      .Get(x => x.FarmId == filter.Id && (!farmPictureTypeId.HasValue || x.PictureTypeId == farmPictureTypeId))
                                      join picture in _pictureRepository
                                      .Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
                                      on farmPic.PictureId equals picture.Id
                                      select new FarmPictureResult
                                      {
                                          FarmId = farmPic.FarmId,
                                          FarmPictureTypeId = farmPic.PictureTypeId,
                                          PictureId = farmPic.PictureId
                                      }).ToListAsync();
            return farmPictures;
        }

        public async Task<IList<FarmPictureResult>> GetFarmPicturesByFarmIdsAsync(IEnumerable<long> farmIds, int farmPictureTypeId, IdRequestFilter<long> filter)
        {
            var deletedStatus = PictureStatus.Deleted.GetCode();
            var inactivedStatus = PictureStatus.Inactived.GetCode();
            var farmPictures = await (from farmPic in _farmPictureRepository.Get(x => x.FarmId.In(farmIds) && x.PictureTypeId == farmPictureTypeId)
                                      join picture in _pictureRepository
                                      .Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
                                      on farmPic.PictureId equals picture.Id
                                      select new FarmPictureResult
                                      {
                                          FarmId = farmPic.FarmId,
                                          FarmPictureTypeId = farmPic.PictureTypeId,
                                          PictureId = farmPic.PictureId
                                      }).ToListAsync();
            return farmPictures;
        }

        public async Task<bool> CreateAsync(FarmPicturesModifyRequest request)
        {
            var index = 0;
            foreach (var picture in request.Pictures)
            {
                var binaryPicture = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                var pictureData = Convert.FromBase64String(binaryPicture);
                var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                {
                    CreatedById = request.UpdatedById,
                    CreatedDate = request.CreatedDate,
                    FileName = picture.FileName,
                    MimeType = picture.ContentType,
                    UpdatedById = request.UpdatedById,
                    UpdatedDate = request.UpdatedDate,
                    BinaryData = pictureData,
                    StatusId = PictureStatus.Pending.GetCode()
                });

                var farmPictureType = index == 0 ? (int)FarmPictureType.Thumbnail : (int)FarmPictureType.Secondary;
                await _farmPictureRepository.AddAsync(new FarmPicture()
                {
                    FarmId = request.FarmId,
                    PictureId = pictureId,
                    PictureTypeId = farmPictureType
                });
                index += 1;
            }

            return true;
        }

        public async Task<bool> UpdateAsync(FarmPicturesModifyRequest request)
        {
            var pictureIds = request.Pictures.Select(x => x.Id);
            var deleteFarmPictures = _farmPictureRepository
                        .Get(x => x.FarmId == request.FarmId && x.PictureId.NotIn(pictureIds));

            var deletePictureIds = deleteFarmPictures.Select(x => x.PictureId).ToList();
            if (deletePictureIds.Any())
            {
                await deleteFarmPictures.DeleteAsync();

                await _pictureRepository.Get(x => x.Id.In(deletePictureIds))
                    .DeleteAsync();
            }

            var pictureTypeId = (int)FarmPictureType.Thumbnail;
            var shouldAddPicture = true;
            var hasPicture = _farmPictureRepository.Get(x => x.FarmId == request.FarmId && x.PictureTypeId == pictureTypeId).Any();
            if (hasPicture)
            {
                shouldAddPicture = false;
            }

            foreach (var picture in request.Pictures)
            {
                if (!string.IsNullOrEmpty(picture.Base64Data))
                {
                    var base64Data = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                    var pictureData = Convert.FromBase64String(base64Data);
                    var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                    {
                        CreatedById = request.UpdatedById,
                        CreatedDate = request.CreatedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = request.UpdatedById,
                        UpdatedDate = request.UpdatedDate,
                        BinaryData = pictureData,
                        StatusId = PictureStatus.Pending.GetCode()
                    });

                    var farmPictureType = shouldAddPicture ? pictureTypeId : (int)FarmPictureType.Secondary;
                    _farmPictureRepository.Add(new FarmPicture()
                    {
                        FarmId = request.FarmId,
                        PictureId = pictureId,
                        PictureTypeId = farmPictureType
                    });
                    shouldAddPicture = false;
                }
            }

            var firstRestPicture = await _farmPictureRepository.FirstOrDefaultAsync(x => x.FarmId == request.FarmId && x.PictureTypeId != pictureTypeId);
            if (firstRestPicture != null)
            {
                firstRestPicture.PictureTypeId = pictureTypeId;
                await _farmPictureRepository.UpdateAsync(firstRestPicture);
            }

            return true;
        }

        public async Task<bool> DeleteByFarmIdAsync(long farmId)
        {
            var farmPictures = _farmPictureRepository.Get(x => x.FarmId == farmId);
            var pictureIds = farmPictures.Select(x => x.PictureId).ToList();
            await farmPictures.DeleteAsync();

            await _pictureRepository.Get(x => x.Id.In(pictureIds))
                .DeleteAsync();

            return true;
        }

        public async Task<bool> UpdateStatusByFarmIdAsync(FarmPicturesModifyRequest request, PictureStatus pictureStatus)
        {
            await (from farmPicture in _farmPictureRepository.Get(x => x.FarmId == request.FarmId)
                   join picture in _pictureRepository.Table
                   on farmPicture.PictureId equals picture.Id
                   select picture)
                .Set(x => x.StatusId, pictureStatus.GetCode())
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }
    }
}
