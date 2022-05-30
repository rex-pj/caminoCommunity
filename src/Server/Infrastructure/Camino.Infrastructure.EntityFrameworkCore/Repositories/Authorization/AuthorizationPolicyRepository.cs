using Camino.Core.Contracts.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Requests.Authorization;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class AuthorizationPolicyRepository : IAuthorizationPolicyRepository, IScopedDependency
    {
        private readonly IEntityRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IAppDbContext _appDbContext;
        public AuthorizationPolicyRepository(IEntityRepository<AuthorizationPolicy> authorizationPolicyRepository,
            IRepository<User> userRepository, IAppDbContext appDbContext)
        {
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _userRepository = userRepository;
            _appDbContext = appDbContext;
        }

        public AuthorizationPolicyResult Find(short id)
        {
            var authorizationPolicy = (from policy in _authorizationPolicyRepository.Table
                                       join createdBy in _userRepository.Table
                                       on policy.CreatedById equals createdBy.Id
                                       join updatedBy in _userRepository.Table
                                       on policy.UpdatedById equals updatedBy.Id
                                       where policy.Id == id
                                       select new AuthorizationPolicyResult()
                                       {
                                           CreatedById = policy.CreatedById,
                                           CreatedByName = createdBy.Lastname + " " + createdBy.Firstname,
                                           CreatedDate = policy.CreatedDate,
                                           UpdatedById = policy.UpdatedById,
                                           UpdatedByName = updatedBy.Lastname + " " + updatedBy.Firstname,
                                           UpdatedDate = policy.UpdatedDate,
                                           Description = policy.Description,
                                           Id = policy.Id,
                                           Name = policy.Name
                                       }).FirstOrDefault();

            if (authorizationPolicy == null)
            {
                return null;
            }

            return authorizationPolicy;
        }

        public async Task<AuthorizationPolicyResult> FindByNameAsync(string name)
        {
            var policy = await _authorizationPolicyRepository.Get(x => x.Name == name).Select(x => new AuthorizationPolicyResult
            {
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                UpdatedById = x.UpdatedById,
                UpdatedDate = x.UpdatedDate
            }).FirstOrDefaultAsync();

            return policy;
        }

        public BasePageList<AuthorizationPolicyResult> Get(AuthorizationPolicyFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = (from policy in _authorizationPolicyRepository.Table
                         join createdBy in _userRepository.Table
                         on policy.CreatedById equals createdBy.Id
                         join updatedBy in _userRepository.Table
                         on policy.UpdatedById equals updatedBy.Id
                         where string.IsNullOrEmpty(search) || policy.Name.ToLower().Contains(search)
                         || (policy.Description != null && policy.Description.ToLower().Contains(search))
                         select new AuthorizationPolicyResult()
                         {
                             CreatedById = policy.CreatedById,
                             CreatedByName = createdBy.Lastname + " " + createdBy.Firstname,
                             CreatedDate = policy.CreatedDate,
                             UpdatedById = policy.UpdatedById,
                             UpdatedByName = updatedBy.Lastname + " " + updatedBy.Firstname,
                             UpdatedDate = policy.UpdatedDate,
                             Description = policy.Description,
                             Id = policy.Id,
                             Name = policy.Name
                         });

            var filteredNumber = query.Select(x => x.Id).Count();

            var authorizationPolicies = query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize)
                                         .ToList();

            var result = new BasePageList<AuthorizationPolicyResult>(authorizationPolicies)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public long Create(AuthorizationPolicyRequest request)
        {
            var newPolicy = new AuthorizationPolicy
            {
                CreatedById = request.CreatedById,
                CreatedDate = DateTime.UtcNow,
                Description = request.Description,
                Name = request.Name,
                UpdatedById = request.UpdatedById,
                UpdatedDate = DateTime.UtcNow
            };

            _authorizationPolicyRepository.Insert(newPolicy);
            _appDbContext.SaveChanges();
            return newPolicy.Id;
        }

        public async Task<bool> UpdateAsync(AuthorizationPolicyRequest request)
        {
            var existing = await _authorizationPolicyRepository.FindAsync(x => x.Id == request.Id);

            existing.Description = request.Description;
            existing.Name = request.Name;
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTime.UtcNow;

            return (await _appDbContext.SaveChangesAsync()) > 0;
        }

    }
}
