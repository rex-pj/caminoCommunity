using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Users;
using Microsoft.EntityFrameworkCore;

namespace Camino.Application.AppServices.Authorization
{
    public class RoleAppService : IRoleAppService, IScopedDependency
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserAppService _userAppService;
        private readonly IEntityRepository<Role> _roleEntityRepository;
        private readonly IEntityRepository<User> _userEntityRepository;

        public RoleAppService(IEntityRepository<Role> roleEntityRepository,
            IEntityRepository<User> userEntityRepository,
            IRoleRepository roleRepository, IUserAppService userAppService)
        {
            _roleEntityRepository = roleEntityRepository;
            _userEntityRepository = userEntityRepository;
            _roleRepository = roleRepository;
            _userAppService = userAppService;
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
                UpdatedById = request.UpdatedById,
                Name = request.Name,
                Description = request.Description
            };
            return await _roleRepository.CreateAsync(role);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _roleRepository.DeleteAsync(id);
        }

        public async Task<RoleResult> FindAsync(long id)
        {
            var existing = await _roleRepository.FindAsync(id);
            if(existing == null)
            {
                return null;
            }

            var result = MapEntityToDto(existing);
            var createdByUser = await _userAppService.FindByIdAsync(existing.CreatedById);
            var updatedByUser = await _userAppService.FindByIdAsync(existing.UpdatedById);
            result.CreatedByName = createdByUser?.DisplayName;
            result.UpdatedByName = updatedByUser?.DisplayName;

            return result;
        }

        private RoleResult MapEntityToDto(Role role)
        {
            return new RoleResult
            {
                CreatedById = role.CreatedById,
                CreatedDate = role.CreatedDate,
                Description = role.Description,
                Id = role.Id,
                Name = role.Name,
                UpdatedById = role.UpdatedById,
                UpdatedDate = role.UpdatedDate,
                ConcurrencyStamp = role.ConcurrencyStamp
            };
        }

        public List<RoleResult> Search(BaseFilter filter, List<long> currentRoleIds = null)
        {
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var hasCurrentRoleIds = currentRoleIds != null && currentRoleIds.Any();
            var query = _roleEntityRepository.Get(x => string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword))
                .Where(x => !hasCurrentRoleIds || !currentRoleIds.Contains(x.Id));

            if (filter.PageSize > 0)
            {
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var users = query
                .Select(x => new RoleResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return users;
        }

        public async Task<RoleResult> FindByNameAsync(string name)
        {
            var existing = await _roleRepository.FindByNameAsync(name);
            if (existing == null)
            {
                return null;
            }

            return MapEntityToDto(existing);
        }

        public async Task<BasePageList<RoleResult>> GetAsync(RoleFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = (from role in _roleEntityRepository.Table
                         join createdBy in _userEntityRepository.Table
                         on role.CreatedById equals createdBy.Id
                         join updatedBy in _userEntityRepository.Table
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

        public async Task<bool> UpdateAsync(RoleModifyRequest request)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Role Id");
            }

            var existing = _roleEntityRepository.Find(x => x.Id == request.Id);
            existing.Description = request.Description;
            existing.Name = request.Name;
            existing.UpdatedById = request.UpdatedById;
            existing.ConcurrencyStamp = request.ConcurrencyStamp;
            return await _roleRepository.UpdateAsync(existing);
        }
        #endregion
    }
}
