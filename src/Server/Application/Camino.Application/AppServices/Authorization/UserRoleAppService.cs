using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Camino.Application.AppServices.Authorization
{
    public class UserRoleAppService : IUserRoleAppService, IScopedDependency
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IEntityRepository<UserRole> _userRoleEntityRepository;

        public UserRoleAppService(IEntityRepository<UserRole> userRoleEntityRepository,
            IUserRoleRepository userRoleRepository)
        {
            _userRoleEntityRepository = userRoleEntityRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IList<UserRoleResult>> GetUserRolesAsync(long userId)
        {
            var userRoles = await _userRoleRepository.GetListAsync(userId);
            if(userRoles == null)
            {
                return new List<UserRoleResult>();
            }

            return userRoles.Select(x => MapEntityToDto(x)).ToList();
        }

        public async Task<UserRoleResult> FindUserRoleAsync(long userId, long roleId)
        {
            var existing = await _userRoleRepository.FindAsync(userId, roleId);
            if(existing == null)
            {
                return null;
            }

            return MapEntityToDto(existing);
        }

        public async Task<IList<UserResult>> GetUsersInRoleAsync(long roleId)
        {
            var existUserRoles = await _userRoleEntityRepository.Get(x => x.RoleId == roleId, x => x.User)
                .Select(x => new UserResult()
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    Lastname = x.Lastname,
                    Firstname = x.Firstname,
                    UserName = x.UserName,
                    Email = x.Email,
                    IsEmailConfirmed = x.IsEmailConfirmed,
                    StatusId = x.StatusId
                }).ToListAsync();

            return existUserRoles;
        }

        private UserRoleResult MapEntityToDto(UserRole userRole)
        {
            return new UserRoleResult
            {
                RoleId = userRole.RoleId,
                RoleName = userRole.Role.Name,
                UserId = userRole.UserId
            };
        }

        public async Task<bool> CreateAsync(UserRoleRequest request)
        {
            var userRole = new UserRole
            {
                RoleId = request.RoleId,
                UserId = request.UserId
            };
            return await _userRoleRepository.CreateAsync(userRole);
        }

        public async Task<bool> RemoveAsync(UserRoleRequest request)
        {
            return await _userRoleRepository.RemoveAsync(request.RoleId, request.UserId);
        }
    }
}
