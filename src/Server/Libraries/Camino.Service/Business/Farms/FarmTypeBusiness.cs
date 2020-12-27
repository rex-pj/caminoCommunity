using Camino.Data.Contracts;
using Camino.Service.Projections.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Farm;
using Camino.Service.Business.Farms.Contracts;

namespace Camino.Service.Business.Farms
{
    public class FarmTypeBusiness : IFarmTypeBusiness
    {
        private readonly IRepository<FarmType> _farmTypeRepository;
        private readonly IRepository<User> _userRepository;

        public FarmTypeBusiness(IRepository<FarmType> farmTypeRepository,
            IRepository<User> userRepository)
        {
            _farmTypeRepository = farmTypeRepository;
            _userRepository = userRepository;
        }

        public FarmTypeProjection Find(long id)
        {
            var exist = (from farmType in _farmTypeRepository.Table
                         where farmType.Id == id
                         select new FarmTypeProjection
                         {
                             Description = farmType.Description,
                             CreatedDate = farmType.CreatedDate,
                             CreatedById = farmType.CreatedById,
                             Id = farmType.Id,
                             Name = farmType.Name,
                             UpdatedById = farmType.UpdatedById,
                             UpdatedDate = farmType.UpdatedDate,
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            var createdByUser = _userRepository.FirstOrDefault(x => x.Id == exist.CreatedById);
            var updatedByUser = _userRepository.FirstOrDefault(x => x.Id == exist.UpdatedById);

            exist.CreatedBy = createdByUser.DisplayName;
            exist.UpdatedBy = updatedByUser.DisplayName;

            return exist;
        }

        public FarmTypeProjection FindByName(string name)
        {
            var farmType = _farmTypeRepository.Get(x => x.Name == name)
                .Select(x => new FarmTypeProjection()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefault();

            return farmType;
        }

        public async Task<BasePageList<FarmTypeProjection>> GetAsync(FarmTypeFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var farmTypeQuery = _farmTypeRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                farmTypeQuery = farmTypeQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var query = farmTypeQuery.Select(a => new FarmTypeProjection
            {
                CreatedById = a.CreatedById,
                CreatedDate = a.CreatedDate,
                Description = a.Description,
                Id = a.Id,
                Name = a.Name,
                UpdatedById = a.UpdatedById,
                UpdatedDate = a.UpdatedDate
            });

            var filteredNumber = query.Select(x => x.Id).Count();

            var farmTypes = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var createdByIds = farmTypes.Select(x => x.CreatedById).ToArray();
            var updatedByIds = farmTypes.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).ToList();

            foreach (var farmType in farmTypes)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == farmType.CreatedById);
                farmType.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == farmType.CreatedById);
                farmType.UpdatedBy = updatedBy.DisplayName;
            }


            var result = new BasePageList<FarmTypeProjection>(farmTypes);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }

        public List<FarmTypeProjection> Get(Expression<Func<FarmType, bool>> filter)
        {
            var farmTypes = _farmTypeRepository.Get(filter).Select(a => new FarmTypeProjection
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            return farmTypes;
        }

        public async Task<IList<FarmTypeProjection>> SearchAsync(string search = "", int page = 1, int pageSize = 10)
        {
            if (search == null)
            {
                search = string.Empty;
            }

            search = search.ToLower();
            var query = _farmTypeRepository.Table
                .Select(c => new FarmTypeProjection
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

            var farmTypes = await query
                .Select(x => new FarmTypeProjection()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return farmTypes;
        }

        public int Create(FarmTypeProjection farmType)
        {
            var newFarmType = new FarmType()
            {
                Name = farmType.Name,
                Description = farmType.Description,
                CreatedById = farmType.CreatedById,
                UpdatedById = farmType.UpdatedById,
            };
            newFarmType.UpdatedDate = DateTimeOffset.UtcNow;
            newFarmType.CreatedDate = DateTimeOffset.UtcNow;

            var id = _farmTypeRepository.AddWithInt32Entity(newFarmType);
            return id;
        }

        public FarmTypeProjection Update(FarmTypeProjection farmType)
        {
            var exist = _farmTypeRepository.FirstOrDefault(x => x.Id == farmType.Id);
            exist.Description = farmType.Description;
            exist.Name = farmType.Name;
            exist.UpdatedById = farmType.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;

            _farmTypeRepository.Update(exist);

            farmType.UpdatedDate = exist.UpdatedDate;
            return farmType;
        }
    }
}
