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
        private readonly IRepository<Picture> _pictureRepository;

        public FarmBusiness(IRepository<Farm> farmRepository, IRepository<FarmType> farmTypeRepository,
            IRepository<FarmPicture> farmPictureRepository, IRepository<User> userRepository,
            IRepository<Picture> pictureRepository)
        {
            _farmRepository = farmRepository;
            _farmTypeRepository = farmTypeRepository;
            _farmPictureRepository = farmPictureRepository;
            _userRepository = userRepository;
            _pictureRepository = pictureRepository;
        }

        public FarmProjection Find(long id)
        {
            var exist = (from farm in _farmRepository.Table
                         join farmType in _farmTypeRepository.Table
                         on farm.FarmTypeId equals farmType.Id
                         where farm.Id == id
                         select new FarmProjection
                         {
                             CreatedDate = farm.CreatedDate,
                             CreatedById = farm.CreatedById,
                             Id = farm.Id,
                             Name = farm.Name,
                             UpdatedById = farm.UpdatedById,
                             UpdatedDate = farm.UpdatedDate,
                             Description = farm.Description,
                             FarmTypeId = farm.FarmTypeId,
                             FarmTypeName = farmType.Name
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            return exist;
        }

        public FarmProjection FindDetail(long id)
        {
            var exist = (from farm in _farmRepository.Table
                         join farmType in _farmTypeRepository.Table
                         on farm.FarmTypeId equals farmType.Id
                         where farm.Id == id
                         select new FarmProjection
                         {
                             Description = farm.Description,
                             CreatedDate = farm.CreatedDate,
                             CreatedById = farm.CreatedById,
                             Id = farm.Id,
                             Name = farm.Name,
                             UpdatedById = farm.UpdatedById,
                             UpdatedDate = farm.UpdatedDate,
                             FarmTypeName = farmType.Name,
                             FarmTypeId = farm.FarmTypeId
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            var farmPictures = (from farmPic in _farmPictureRepository.Get(x => x.FarmId == id)
                                join farm in _farmRepository.Table
                                on farmPic.PictureId equals farm.Id
                                orderby farm.CreatedDate descending
                                select new PictureRequestProjection()
                                {
                                    Id = farmPic.PictureId
                                }).ToList();

            if (farmPictures.Any())
            {
                exist.Pictures = farmPictures;
            }

            var createdByUser = _userRepository.FirstOrDefault(x => x.Id == exist.CreatedById);
            var updatedByUser = _userRepository.FirstOrDefault(x => x.Id == exist.UpdatedById);

            exist.CreatedBy = createdByUser.DisplayName;
            exist.UpdatedBy = updatedByUser.DisplayName;

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

            var thumbnailType = (int)FarmPictureType.Thumbnail;
            var query = from ar in farmQuery
                        join pic in _farmPictureRepository.Table
                        on ar.Id equals pic.FarmId into pics
                        from p in pics.DefaultIfEmpty()
                        where p == null || p.PictureType == thumbnailType
                        select new FarmProjection
                        {
                            Id = ar.Id,
                            Name = ar.Name,
                            CreatedById = ar.CreatedById,
                            CreatedDate = ar.CreatedDate,
                            Description = ar.Description,
                            UpdatedById = ar.UpdatedById,
                            UpdatedDate = ar.UpdatedDate,
                            Pictures = new List<PictureRequestProjection>()
                            {
                                new PictureRequestProjection()
                                {
                                    Id = p.PictureId
                                }
                            }
                        };

            var farms = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var createdByIds = farms.Select(x => x.CreatedById).ToArray();
            var updatedByIds = farms.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).ToList();

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
                UpdatedById = farm.UpdatedById,
                CreatedById = farm.CreatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Description = farm.Description
            };
            newFarm.UpdatedDate = modifiedDate;
            newFarm.CreatedDate = modifiedDate;

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

            int index = 0;
            foreach (var picture in request.Pictures)
            {
                if (!string.IsNullOrEmpty(picture.Base64Data))
                {
                    var farmPictures = _farmPictureRepository
                        .Get(x => x.FarmId == request.Id)
                        .AsEnumerable();

                    if (farmPictures.Any())
                    {
                        var pictureIds = farmPictures.Select(x => x.PictureId).ToList();
                        await _farmPictureRepository.DeleteAsync(farmPictures.AsQueryable());

                        var currentThumbnails = _pictureRepository.Get(x => pictureIds.Contains(x.Id));
                        await _pictureRepository.DeleteAsync(currentThumbnails);
                    }

                    var pictureData = Convert.FromBase64String(picture.Base64Data);
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

                    var farmPictureType = index == 0 ? (int)FarmPictureType.Thumbnail : (int)FarmPictureType.Secondary;
                    _farmPictureRepository.Add(new FarmPicture()
                    {
                        FarmId = farm.Id,
                        PictureId = pictureId,
                        PictureType = farmPictureType
                    });
                    index += 1;
                }

                _farmRepository.Update(farm);
            }

            request.UpdatedDate = farm.UpdatedDate;
            return request;
        }
    }
}
