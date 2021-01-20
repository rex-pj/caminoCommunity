using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Service.Business.Farms
{
    public class FarmPictureBusiness : IFarmPictureBusiness
    {
        private readonly IRepository<FarmPicture> _farmPictureRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<User> _userRepository;

        public FarmPictureBusiness(IRepository<FarmPicture> farmPictureRepository, IRepository<Picture> pictureRepository,
            IRepository<User> userRepository, IRepository<Farm> farmRepository)
        {
            _farmPictureRepository = farmPictureRepository;
            _pictureRepository = pictureRepository;
            _farmRepository = farmRepository;
            _userRepository = userRepository;
        }

        public async Task<BasePageList<FarmPictureProjection>> GetAsync(FarmPictureFilter filter)
        {
            var pictureQuery = _pictureRepository.Get(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var search = filter.Search.ToLower();
                pictureQuery = pictureQuery.Where(pic => pic.Title.ToLower().Contains(search)
                         || pic.Title.ToLower().Contains(search));
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
                        select new FarmPictureProjection()
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

            var createdByIds = farmPictures.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();

            foreach (var farmPicture in farmPictures)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == farmPicture.PictureCreatedById);
                farmPicture.PictureCreatedBy = createdBy.DisplayName;
            }

            var result = new BasePageList<FarmPictureProjection>(farmPictures)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }
    }
}
