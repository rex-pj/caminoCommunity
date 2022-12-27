using System;
using System.Threading.Tasks;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class RoleAuthorizationPolicyRepository : IRoleAuthorizationPolicyRepository, IScopedDependency
    {
        private readonly IRepository<RoleAuthorizationPolicy> _roleAuthorizationPolicyEntityRepository;
        private readonly IRepository<AuthorizationPolicy> _authorizationPolicyEntityRepository;
        private readonly IRepository<Role> _roleEntityRepository;
        private readonly IDbContext _dbContext;

        public RoleAuthorizationPolicyRepository(IRepository<RoleAuthorizationPolicy> roleAuthorizationPolicyEntityRepository,
            IRepository<AuthorizationPolicy> authorizationPolicyEntityRepository,
            IRepository<Role> userEntityRepository, IDbContext dbContext)
        {
            _roleAuthorizationPolicyEntityRepository = roleAuthorizationPolicyEntityRepository;
            _authorizationPolicyEntityRepository = authorizationPolicyEntityRepository;
            _roleEntityRepository = userEntityRepository;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(RoleAuthorizationPolicy roleAuthorizationPolicy)
        {
            roleAuthorizationPolicy.GrantedDate = DateTime.UtcNow;
            _roleAuthorizationPolicyEntityRepository.Insert(roleAuthorizationPolicy);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(long roleId, long authorizationPolicyId)
        {
            var role = _roleEntityRepository.Find(x => x.Id == roleId);
            if (role == null)
            {
                return false;
            }

            var authorizationPolicy = _authorizationPolicyEntityRepository.Find(x => x.Id == authorizationPolicyId);
            if (authorizationPolicy == null)
            {
                return false;
            }

            _roleAuthorizationPolicyEntityRepository.Delete(x => x.RoleId == roleId && x.AuthorizationPolicyId == authorizationPolicyId);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
