using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Users;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class UserAuthorizationPolicyRepository : IUserAuthorizationPolicyRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserAuthorizationPolicy> _userAuthorizationPolicyEntityRepository;
        private readonly IEntityRepository<RoleAuthorizationPolicy> _roleAuthorizationPolicyRepository;
        private readonly IEntityRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IEntityRepository<UserRole> _userRoleRepository;
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IDbContext _dbContext;

        public UserAuthorizationPolicyRepository(IEntityRepository<UserAuthorizationPolicy> userAuthorizationPolicyEntityRepository,
            IEntityRepository<AuthorizationPolicy> authorizationPolicyEntityRepository, IEntityRepository<User> userEntityRepository,
            IEntityRepository<UserRole> userRoleEntityRepository, IEntityRepository<Role> roleEntityRepository,
            IEntityRepository<RoleAuthorizationPolicy> roleAuthorizationPolicyEntityRepository,
            IDbContext dbContext)
        {
            _userAuthorizationPolicyEntityRepository = userAuthorizationPolicyEntityRepository;
            _authorizationPolicyRepository = authorizationPolicyEntityRepository;
            _userRepository = userEntityRepository;
            _userRoleRepository = userRoleEntityRepository;
            _roleRepository = roleEntityRepository;
            _roleAuthorizationPolicyRepository = roleAuthorizationPolicyEntityRepository;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(UserAuthorizationPolicy userAuthorizationPolicy)
        {
            userAuthorizationPolicy.GrantedDate = DateTime.UtcNow;
            _userAuthorizationPolicyEntityRepository.Insert(userAuthorizationPolicy);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(long userId, short authorizationPolicyId)
        {
            var user = await _userRepository.FindAsync(x => x.Id == userId);
            if (user == null)
            {
                return false;
            }

            var authorizationPolicy = await _authorizationPolicyRepository.FindAsync(x => x.Id == authorizationPolicyId);
            if (authorizationPolicy == null)
            {
                return false;
            }

            await _userAuthorizationPolicyEntityRepository.DeleteAsync(x => x.UserId == userId && x.AuthorizationPolicyId == authorizationPolicyId);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId)
        {
            var isUserHasPolicy = await _userAuthorizationPolicyEntityRepository
                .Get(x => x.UserId == userId && x.AuthorizationPolicyId == policyId)
                .AnyAsync();

            if (isUserHasPolicy)
            {
                return true;
            }

            return (from role in _roleRepository.Table
                    join userRole in _userRoleRepository.Table
                    on role.Id equals userRole.RoleId
                    join roleAuthorization in _roleAuthorizationPolicyRepository.Table
                    on role.Id equals roleAuthorization.RoleId
                    where userRole.UserId == userId && roleAuthorization.AuthorizationPolicyId == policyId
                    select role)
                    .Any();
        }
    }
}
