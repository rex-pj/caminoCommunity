﻿using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Core.Domain.Farms;
using Camino.Core.Domain.Media;
using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Shared.Requests.Farms;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Farms
{
    public class FarmPictureRepository : IFarmPictureRepository, IScopedDependency
    {
        private readonly IEntityRepository<FarmPicture> _farmPictureRepository;
        private readonly IEntityRepository<Picture> _pictureRepository;
        private readonly IEntityRepository<Farm> _farmRepository;
        private readonly IAppDbContext _dbContext;

        public FarmPictureRepository(IEntityRepository<FarmPicture> farmPictureRepository, IEntityRepository<Picture> pictureRepository,
           IEntityRepository<Farm> farmRepository, IAppDbContext dbContext)
        {
            _farmPictureRepository = farmPictureRepository;
            _pictureRepository = pictureRepository;
            _farmRepository = farmRepository;
            _dbContext = dbContext;
        }

        public async Task<BasePageList<FarmPictureResult>> GetAsync(FarmPictureFilter filter)
        {
            var pictureQuery = _pictureRepository.Get(x => x.StatusId != PictureStatus.Deleted.GetCode());
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

        public async Task<IList<FarmPictureResult>> GetFarmPicturesByFarmIdsAsync(IEnumerable<long> farmIds, IdRequestFilter<long> filter, FarmPictureType farmPictureType)
        {
            var deletedStatus = PictureStatus.Deleted.GetCode();
            var inactivedStatus = PictureStatus.Inactived.GetCode();
            var farmPictureTypeId = farmPictureType.GetCode();
            var farmPictures = await (from farmPic in _farmPictureRepository.Get(x => farmIds.Contains(x.FarmId) && x.PictureTypeId == farmPictureTypeId)
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

                var newPicture = new Picture()
                {
                    CreatedById = request.UpdatedById,
                    CreatedDate = request.CreatedDate,
                    FileName = picture.FileName,
                    MimeType = picture.ContentType,
                    UpdatedById = request.UpdatedById,
                    UpdatedDate = request.UpdatedDate,
                    BinaryData = pictureData,
                    StatusId = PictureStatus.Pending.GetCode()
                };
                await _pictureRepository.InsertAsync(newPicture);
                await _dbContext.SaveChangesAsync();

                var farmPictureType = index == 0 ? (int)FarmPictureType.Thumbnail : (int)FarmPictureType.Secondary;
                await _farmPictureRepository.InsertAsync(new FarmPicture()
                {
                    FarmId = request.FarmId,
                    PictureId = newPicture.Id,
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
                        .Get(x => x.FarmId == request.FarmId && !pictureIds.Contains(x.PictureId));

            var deletePictureIds = deleteFarmPictures.Select(x => x.PictureId).ToList();
            if (deletePictureIds.Any())
            {
                await _farmPictureRepository.DeleteAsync(deleteFarmPictures);
                await _pictureRepository.DeleteAsync(x => deletePictureIds.Contains(x.Id));
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

                    var newPicture = new Picture()
                    {
                        CreatedById = request.UpdatedById,
                        CreatedDate = request.CreatedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = request.UpdatedById,
                        UpdatedDate = request.UpdatedDate,
                        BinaryData = pictureData,
                        StatusId = PictureStatus.Pending.GetCode()
                    };
                    await _pictureRepository.InsertAsync(newPicture);

                    var farmPictureType = shouldAddPicture ? pictureTypeId : (int)FarmPictureType.Secondary;
                    _farmPictureRepository.Insert(new FarmPicture()
                    {
                        FarmId = request.FarmId,
                        PictureId = newPicture.Id,
                        PictureTypeId = farmPictureType
                    });
                    
                    shouldAddPicture = false;
                }
            }

            var firstRestPicture = await _farmPictureRepository.FindAsync(x => x.FarmId == request.FarmId && x.PictureTypeId != pictureTypeId);
            if (firstRestPicture != null)
            {
                firstRestPicture.PictureTypeId = pictureTypeId;
                await _farmPictureRepository.UpdateAsync(firstRestPicture);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByFarmIdAsync(long farmId)
        {
            var farmPictures = _farmPictureRepository.Get(x => x.FarmId == farmId);
            var pictureIds = farmPictures.Select(x => x.PictureId).ToList();
            await _farmPictureRepository.DeleteAsync(farmPictures);

            await _pictureRepository.DeleteAsync(x => pictureIds.Contains(x.Id));
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusByFarmIdAsync(FarmPicturesModifyRequest request, PictureStatus pictureStatus)
        {
            var existingPictures = (from farmPicture in _farmPictureRepository.Get(x => x.FarmId == request.FarmId)
                                          join picture in _pictureRepository.Table
                                          on farmPicture.PictureId equals picture.Id
                                          select picture);

            foreach (var picture in existingPictures)
            {
                picture.StatusId = pictureStatus.GetCode();
                picture.UpdatedById = request.UpdatedById;
                picture.UpdatedDate = DateTimeOffset.UtcNow;
            }

            await _pictureRepository.UpdateAsync(existingPictures);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
