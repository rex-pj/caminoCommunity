using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class AuthorizationPolicyRepository : IAuthorizationPolicyRepository, IScopedDependency
    {
        private readonly IEntityRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IDbContext _appDbContext;
        public AuthorizationPolicyRepository(IEntityRepository<AuthorizationPolicy> authorizationPolicyRepository,
           IDbContext appDbContext)
        {
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _appDbContext = appDbContext;
        }

        public async Task<AuthorizationPolicy> FindAsync(short id)
        {
            var authorizationPolicy = await _authorizationPolicyRepository.FindAsync(x => x.Id == id);
            return authorizationPolicy;
        }

        public async Task<AuthorizationPolicy> FindByNameAsync(string name)
        {
            var policy = await _authorizationPolicyRepository.FindAsync(x => x.Name == name);
            return policy;
        }

        public async Task<long> CreateAsync(AuthorizationPolicy authorizationPolicy)
        {
            var modifiedDate = DateTime.UtcNow;
            authorizationPolicy.UpdatedDate = modifiedDate;
            authorizationPolicy.CreatedDate = modifiedDate;

            _authorizationPolicyRepository.Insert(authorizationPolicy);
            await _appDbContext.SaveChangesAsync();
            return authorizationPolicy.Id;
        }

        public async Task<bool> UpdateAsync(AuthorizationPolicy authorizationPolicy)
        {
            authorizationPolicy.UpdatedDate = DateTime.UtcNow;
            return (await _appDbContext.SaveChangesAsync()) > 0;
        }

    }
}
