using AutoMapper;
using Camino.Data.Contracts;
using Camino.Service.Data.Identity;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Authorization.Contracts;
using Camino.IdentityDAL.Entities;

namespace Camino.Service.Business.Authorization
{
    public class UserRoleBusiness : IUserRoleBusiness
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;

        public UserRoleBusiness(IRepository<UserRole> userRoleRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IList<UserRoleProjection>> GetUserRolesAsync(long userId)
        {
            var userRoles = await _userRoleRepository.Get(x => x.UserId == userId)
                .Select(x => new UserRoleProjection()
                {
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name,
                    UserId = x.UserId
                }).ToListAsync();

            return userRoles;
        }

        public async Task<UserRoleProjection> FindUserRoleAsync(long userId, long roleId)
        {
            var userRole = (await _userRoleRepository.GetAsync(x => x.UserId == userId && x.RoleId == roleId))
                .FirstOrDefault();

            return _mapper.Map<UserRoleProjection>(userRole);
        }

        public async Task<IList<UserProjection>> GetUsersInRoleAsync(long roleId)
        {
            var existUserRoles = await _userRoleRepository.Get(x => x.RoleId == roleId)
                .Select(x => new UserProjection()
                {
                    Id = x.UserId,
                    DisplayName = x.User.DisplayName,
                    Lastname = x.User.Lastname,
                    Firstname = x.User.Firstname,
                    UserName = x.User.UserName,
                    Email = x.User.Email,
                    IsEmailConfirmed = x.User.IsEmailConfirmed,
                    StatusId = x.User.StatusId
                }).ToListAsync();

            return existUserRoles;
        }

        public void Add(UserRoleProjection userRoleRequest)
        {
            var userRole = _mapper.Map<UserRole>(userRoleRequest);
            _userRoleRepository.Add(userRole);
        }

        public void Remove(UserRoleProjection userRoleRequest)
        {
            var userRole = _mapper.Map<UserRole>(userRoleRequest);
            _userRoleRepository.Delete(userRole);
        }
    }
}
