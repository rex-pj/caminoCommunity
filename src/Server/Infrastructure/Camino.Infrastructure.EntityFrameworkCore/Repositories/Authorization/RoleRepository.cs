using Camino.Core.Contracts.Data;
using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class RoleRepository : IRoleRepository, IScopedDependency
    {
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IAppDbContext _dbContext;

        public RoleRepository(IEntityRepository<Role> roleRepository, IEntityRepository<User> userRepository, IAppDbContext dbContext)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _dbContext = dbContext;
        }

        #region CRUD
        public async Task<long> CreateAsync(RoleModifyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var role = new Role
            {
                ConcurrencyStamp = request.ConcurrencyStamp,
                CreatedById = request.CreatedById,
                CreatedDate = DateTime.UtcNow,
                UpdatedById = request.UpdatedById,
                UpdatedDate = DateTime.UtcNow,
                Name = request.Name,
                Description = request.Description
            };

            await _roleRepository.InsertAsync(role);
            await _dbContext.SaveChangesAsync();
            return role.Id;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _roleRepository.DeleteAsync(x => x.Id == id);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<RoleResult> FindAsync(long id)
        {
            var existRole = await (from role in _roleRepository.Table
                                   join createdBy in _userRepository.Table
                                   on role.CreatedById equals createdBy.Id
                                   join updatedBy in _userRepository.Table
                                   on role.UpdatedById equals updatedBy.Id
                                   where role.Id == id
                                   select new RoleResult()
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

        public List<RoleResult> Search(BaseFilter filter, List<long> currentRoleIds = null)
        {
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var hasCurrentRoleIds = currentRoleIds != null && currentRoleIds.Any();
            var data = _roleRepository.Get(x => string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword))
                .Where(x => !hasCurrentRoleIds || !currentRoleIds.Contains(x.Id));

            if (filter.PageSize > 0)
            {
                data = data.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var users = data
                .Select(x => new RoleResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return users;
        }

        public RoleResult FindByName(string name)
        {
            var role = _roleRepository.Get(x => x.Name == name)
                .Select(x => new RoleResult
                {
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedById = x.UpdatedById,
                    UpdatedDate = x.UpdatedDate,
                    Name = x.Name,
                    Description = x.Description,
                    Id = x.Id
                })
                .FirstOrDefault();

            return role;
        }

        public async Task<RoleResult> FindByNameAsync(string name)
        {
            name = name.ToLower();
            var role = await _roleRepository.Get(x => x.Name.ToLower() == name)
                .Select(x => new RoleResult
                {
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedById = x.UpdatedById,
                    UpdatedDate = x.UpdatedDate,
                    Name = x.Name,
                    Description = x.Description,
                    Id = x.Id
                })
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<BasePageList<RoleResult>> GetAsync(RoleFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = (from role in _roleRepository.Table
                         join createdBy in _userRepository.Table
                         on role.CreatedById equals createdBy.Id
                         join updatedBy in _userRepository.Table
                         on role.UpdatedById equals updatedBy.Id
                         where string.IsNullOrEmpty(search) || role.Name.ToLower().Contains(search)
                         || (role.Description != null && role.Description.ToLower().Contains(search))
                         select new RoleResult
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

            var result = new BasePageList<RoleResult>(roles)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<bool> UpdateAsync(RoleModifyRequest roleModel)
        {
            if (roleModel.Id <= 0)
            {
                throw new ArgumentException("Role Id");
            }

            var exist = _roleRepository.Find(x => x.Id == roleModel.Id);
            exist.Description = roleModel.Description;
            exist.Name = roleModel.Name;
            exist.UpdatedById = roleModel.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;
            exist.ConcurrencyStamp = roleModel.ConcurrencyStamp;

            await _roleRepository.UpdateAsync(exist);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<RoleResult> GetByNameAsync(string name)
        {
            var role = await _roleRepository.Get(x => x.Name.Equals(name))
                .Select(x => new RoleResult
                {
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedById = x.UpdatedById,
                    UpdatedDate = x.UpdatedDate,
                    Name = x.Name,
                    Description = x.Description,
                    Id = x.Id
                })
                .FirstOrDefaultAsync();

            return role;
        }
        #endregion
    }
}
