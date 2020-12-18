using Camino.Core.Utils;
using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.Data.Enums;
using Camino.IdentityDAL.Entities;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Media;
using Camino.Service.Projections.PageList;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Service.Business.Farms
{
    public class FarmBusiness : IFarmBusiness
    {
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<FarmType> _farmTypeRepository;
        private readonly IRepository<FarmPicture> _farmPictureRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<Picture> _pictureRepository;

        public FarmBusiness(IRepository<Farm> farmRepository, IRepository<FarmType> farmTypeRepository,
            IRepository<FarmPicture> farmPictureRepository, IRepository<User> userRepository,
            IRepository<Picture> pictureRepository, IRepository<UserPhoto> userPhotoRepository)
        {
            _farmRepository = farmRepository;
            _farmTypeRepository = farmTypeRepository;
            _farmPictureRepository = farmPictureRepository;
            _userRepository = userRepository;
            _pictureRepository = pictureRepository;
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<FarmProjection> FindAsync(long id)
        {
            var exist = await (from farm in _farmRepository.Table
                               join farmType in _farmTypeRepository.Table
                               on farm.FarmTypeId equals farmType.Id
                               where farm.Id == id
                               select new FarmProjection
                               {
                                   CreatedDate = farm.CreatedDate,
                                   CreatedById = farm.CreatedById,
                                   Id = farm.Id,
                                   Name = farm.Name,
                                   Address = farm.Address,
                                   UpdatedById = farm.UpdatedById,
                                   UpdatedDate = farm.UpdatedDate,
                                   Description = farm.Description,
                                   FarmTypeId = farm.FarmTypeId,
                                   FarmTypeName = farmType.Name
                               }).FirstOrDefaultAsync();

            return exist;
        }

        public async Task<IList<FarmProjection>> SearchByUserIdAsync(long userId, string search = "", int page = 1, int pageSize = 10)
        {
            if (search == null)
            {
                search = string.Empty;
            }

            search = search.ToLower();
            var query = _farmRepository.Get(x => x.CreatedById == userId)
                .Select(c => new FarmProjection
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                });

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search) || x.Description.ToLower().Contains(search));
            }

            if (pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var farms = await query
                .Select(x => new FarmProjection()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return farms;
        }

        public async Task<FarmProjection> FindDetailAsync(long id)
        {
            var exist = await (from farm in _farmRepository.Table
                               join farmType in _farmTypeRepository.Table
                               on farm.FarmTypeId equals farmType.Id
                               join farmPic in _farmPictureRepository.Table
                               on farm.Id equals farmPic.FarmId into pics
                               where farm.Id == id
                               select new FarmProjection
                               {
                                   Id = farm.Id,
                                   Name = farm.Name,
                                   Address = farm.Address,
                                   Description = farm.Description,
                                   CreatedDate = farm.CreatedDate,
                                   CreatedById = farm.CreatedById,
                                   UpdatedById = farm.UpdatedById,
                                   UpdatedDate = farm.UpdatedDate,
                                   FarmTypeName = farmType.Name,
                                   FarmTypeId = farm.FarmTypeId,
                                   Pictures = pics.Select(x => new PictureRequestProjection
                                   {
                                       Id = x.PictureId
                                   }),
                               }).FirstOrDefaultAsync();

            if (exist == null)
            {
                return null;
            }

            var createdByUserName = await _userRepository.Get(x => x.Id == exist.CreatedById).Select(x => x.DisplayName).FirstOrDefaultAsync();
            exist.CreatedBy = createdByUserName;

            return exist;
        }

        public FarmProjection FindByName(string name)
        {
            var exist = _farmRepository.Get(x => x.Name == name)
                .Select(x => new FarmProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    FarmTypeId = x.FarmTypeId,
                    UpdatedById = x.UpdatedById,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .FirstOrDefault();

            return exist;
        }

        public async Task<BasePageList<FarmProjection>> GetAsync(FarmFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var farmQuery = _farmRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                farmQuery = farmQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.FarmTypeId.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.FarmTypeId == filter.FarmTypeId);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTimeOffset.UtcNow);
            }

            var filteredNumber = farmQuery.Select(x => x.Id).Count();

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var thumbnailTypeId = (int)FarmPictureType.Thumbnail;
            var query = from farm in farmQuery
                        join pic in _farmPictureRepository.Get(x => x.PictureType == thumbnailTypeId)
                        on farm.Id equals pic.FarmId into pics
                        join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                        on farm.CreatedById equals pho.CreatedById into photos
                        from userPhoto in photos.DefaultIfEmpty()
                        select new FarmProjection
                        {
                            Id = farm.Id,
                            Name = farm.Name,
                            Address = farm.Address,
                            CreatedById = farm.CreatedById,
                            CreatedDate = farm.CreatedDate,
                            Description = farm.Description,
                            UpdatedById = farm.UpdatedById,
                            UpdatedDate = farm.UpdatedDate,
                            CreatedByPhotoCode = userPhoto.Code,
                            Pictures = pics.Select(x => new PictureRequestProjection
                            {
                                Id = x.PictureId
                            })
                        };

            var farms = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var createdByIds = farms.Select(x => x.CreatedById).ToArray();
            var updatedByIds = farms.Select(x => x.UpdatedById).ToArray();

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

            foreach (var article in farms)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == article.CreatedById);
                article.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == article.UpdatedById);
                article.UpdatedBy = updatedBy.DisplayName;
            }

            var result = new BasePageList<FarmProjection>(farms)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<long> CreateAsync(FarmProjection farm)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var newFarm = new Farm()
            {
                FarmTypeId = farm.FarmTypeId,
                Name = farm.Name,
                Address = farm.Address,
                UpdatedById = farm.UpdatedById,
                CreatedById = farm.CreatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Description = farm.Description,
            };

            var id = await _farmRepository.AddWithInt64EntityAsync(newFarm);
            if (id > 0)
            {
                var index = 0;
                foreach (var picture in farm.Pictures)
                {
                    var thumbnail = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                    var pictureData = Convert.FromBase64String(thumbnail);
                    var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                    {
                        CreatedById = farm.UpdatedById,
                        CreatedDate = modifiedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = farm.UpdatedById,
                        UpdatedDate = modifiedDate,
                        BinaryData = pictureData
                    });

                    var farmPictureType = index == 0 ? (int)FarmPictureType.Thumbnail : (int)FarmPictureType.Secondary;
                    _farmPictureRepository.Add(new FarmPicture()
                    {
                        FarmId = id,
                        PictureId = pictureId,
                        PictureType = farmPictureType
                    });
                    index += 1;
                }
            }

            return id;
        }

        public async Task<FarmProjection> UpdateAsync(FarmProjection request)
        {
            var updatedDate = DateTimeOffset.UtcNow;
            var farm = _farmRepository.FirstOrDefault(x => x.Id == request.Id);
            farm.Description = request.Description;
            farm.Name = request.Name;
            farm.FarmTypeId = request.FarmTypeId;
            farm.UpdatedById = request.UpdatedById;
            farm.UpdatedDate = updatedDate;
            farm.Address = farm.Address;

            var pictureIds = request.Pictures.Select(x => x.Id);
            var deleteFarmPictures = _farmPictureRepository
                        .Get(x => x.FarmId == request.Id && !pictureIds.Contains(x.PictureId));

            var deletePictureIds = deleteFarmPictures.Select(x => x.PictureId).ToList();
            if (deletePictureIds.Any())
            {
                await _farmPictureRepository.DeleteAsync(deleteFarmPictures);

                var currentPictures = _pictureRepository.Get(x => deletePictureIds.Contains(x.Id));
                await _pictureRepository.DeleteAsync(currentPictures);
            }

            var thumbnailType = (int)FarmPictureType.Thumbnail;
            var shouldAddThumbnail = true;
            var hasThumbnail = _farmPictureRepository.Get(x => x.FarmId == request.Id && x.PictureType == thumbnailType).Any();
            if (hasThumbnail)
            {
                shouldAddThumbnail = false;
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
                        CreatedDate = updatedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = request.UpdatedById,
                        UpdatedDate = updatedDate,
                        BinaryData = pictureData
                    });

                    var farmPictureType = shouldAddThumbnail ? thumbnailType : (int)FarmPictureType.Secondary;
                    _farmPictureRepository.Add(new FarmPicture()
                    {
                        FarmId = farm.Id,
                        PictureId = pictureId,
                        PictureType = farmPictureType
                    });
                    shouldAddThumbnail = false;
                }
            }

            var firstRestPicture = await _farmPictureRepository.FirstOrDefaultAsync(x => x.FarmId == request.Id && x.PictureType != thumbnailType);
            if (firstRestPicture != null)
            {
                firstRestPicture.PictureType = thumbnailType;
                await _farmPictureRepository.UpdateAsync(firstRestPicture);
            }

            _farmRepository.Update(farm);

            return request;
        }
    }
}
