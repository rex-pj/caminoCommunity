using AutoMapper;
using Camino.Data.Contracts;
using Camino.Service.Data.Identity;
using Camino.Service.Data.Filters;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Authorization.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Data.PageList;

namespace Camino.Service.Business.Authorization
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public RoleBusiness(IMapper mapper, IRepository<Role> roleRepository, IRepository<User> userRepository)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        #region CRUD
        public async Task<long> AddAsync(RoleProjection roleModel)
        {
            if (roleModel == null)
            {
                throw new ArgumentNullException(nameof(roleModel));
            }

            var role = _mapper.Map<Role>(roleModel);
            role.UpdatedDate = DateTime.UtcNow;
            role.CreatedDate = DateTime.UtcNow;

            role.Id = await _roleRepository.AddWithInt64EntityAsync(role);
            return role.Id;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var role = _roleRepository.FirstOrDefault(x => x.Id == id);
            await _roleRepository.DeleteAsync(role);

            return true;
        }

        public async Task<RoleProjection> FindAsync(long id)
        {
            var existRole = await (from role in _roleRepository.Table
                                   join createdBy in _userRepository.Table
                                   on role.CreatedById equals createdBy.Id
                                   join updatedBy in _userRepository.Table
                                   on role.UpdatedById equals updatedBy.Id
                                   where role.Id == id
                                   select new RoleProjection()
                                   {
                                       CreatedByName = createdBy.Lastname + " " + createdBy.Firstname,
                                       UpdatedByName = updatedBy.Lastname + " " + updatedBy.Firstname,
                                       CreatedById = role.CreatedById,
                                       CreatedDate = role.CreatedDate,
                                       Description = role.Description,
                                       Id = role.Id,
                                       Name = role.Name,
                                       UpdatedById = role.UpdatedById,
                                       UpdatedDate = role.UpdatedDate,
                                       ConcurrencyStamp = role.ConcurrencyStamp
                                   }).FirstOrDefaultAsync();

            if (existRole == null)
            {
                return null;
            }

            return existRole;
        }

        public List<RoleProjection> Search(string query = "", List<long> currentRoleIds = null, int page = 1, int pageSize = 10)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            query = query.ToLower();

            var hasCurrentRoleIds = currentRoleIds != null && currentRoleIds.Any();
            var data = _roleRepository.Get(x => string.IsNullOrEmpty(query) || x.Name.ToLower().Contains(query))
                .Where(x => !hasCurrentRoleIds || !currentRoleIds.Contains(x.Id));

            if (pageSize > 0)
            {
                data = data.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var users = data
                .Select(x => new RoleProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return users;
        }

        public RoleProjection FindByName(string name)
        {
            var exist = _roleRepository.Get(x => x.Name == name).FirstOrDefault();
            if (exist == null)
            {
                return null;
            }

            return _mapper.Map<RoleProjection>(exist);
        }

        public async Task<RoleProjection> FindByNameAsync(string name)
        {
            name = name.ToLower();
            var exist = await _roleRepository.Get(x => x.Name.ToLower() == name)
                .FirstOrDefaultAsync();
            if (exist == null)
            {
                return null;
            }

            return _mapper.Map<RoleProjection>(exist);
        }

        public async Task<BasePageList<RoleProjection>> GetAsync(RoleFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var query = (from role in _roleRepository.Table
                         join createdBy in _userRepository.Table
                         on role.CreatedById equals createdBy.Id
                         join updatedBy in _userRepository.Table
                         on role.UpdatedById equals updatedBy.Id
                        where string.IsNullOrEmpty(search) || role.Name.ToLower().Contains(search)
                        || (role.Description != null && role.Description.ToLower().Contains(search))
                        select new RoleProjection
                         {
                             Id = role.Id,
                             Name = role.Name,
                             CreatedById = role.CreatedById,
                             CreatedByName = createdBy.Lastname + " " + createdBy.Firstname,
                             CreatedDate = role.CreatedDate,
                             Description = role.Description,
                             UpdatedById = role.UpdatedById,
                             UpdatedByName = updatedBy.Lastname + " " + updatedBy.Firstname,
                             UpdatedDate = role.UpdatedDate
                         });

            var filteredNumber = query.Select(x => x.Id).Count();

            var roles = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize)
                                         .ToListAsync();

            var result = new BasePageList<RoleProjection>(roles);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }

        public async Task<bool> UpdateAsync(RoleProjection roleModel)
        {
            if (roleModel.Id <= 0)
            {
                throw new ArgumentException("Role Id");
            }

            var exist = _roleRepository.FirstOrDefault(x => x.Id == roleModel.Id);
            exist.Description = roleModel.Description;
            exist.Name = roleModel.Name;
            exist.UpdatedById = roleModel.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;
            exist.ConcurrencyStamp = roleModel.ConcurrencyStamp;

            await _roleRepository.UpdateAsync(exist);
            return true;
        }

        public async Task<RoleProjection> GetByNameAsync(string name)
        {
            var role = (await _roleRepository.GetAsync(x => x.Name.Equals(name))).FirstOrDefault();

            var roleModel = _mapper.Map<RoleProjection>(role);
            return roleModel;
        }
        #endregion
    }
}
