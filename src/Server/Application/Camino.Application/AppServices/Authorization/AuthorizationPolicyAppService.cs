using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Users;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Camino.Application.AppServices.Authorization
{
    public class AuthorizationPolicyAppService : IAuthorizationPolicyAppService, IScopedDependency
    {
        private readonly IAuthorizationPolicyRepository _authorizationPolicyRepository;
        private readonly IEntityRepository<AuthorizationPolicy> _authorizationPolicyEntityRepository;
        private readonly IEntityRepository<User> _userEntityRepository;
        private readonly IUserAppService _userAppService;

        public AuthorizationPolicyAppService(IAuthorizationPolicyRepository authorizationPolicyRepository,
            IEntityRepository<AuthorizationPolicy> authorizationPolicyEntityRepository,
            IEntityRepository<User> userEntityRepository, IUserAppService userAppService)
        {
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _authorizationPolicyEntityRepository = authorizationPolicyEntityRepository;
            _userEntityRepository = userEntityRepository;
            _userAppService = userAppService;
        }

        public async Task<AuthorizationPolicyResult> FindAsync(short id)
        {
            var existing = await _authorizationPolicyRepository.FindAsync(id);
            var result = MapEntityToDto(existing);
            var createdByUserName = (await _userAppService.FindByIdAsync(existing.CreatedById)).DisplayName;
            result.CreatedByName = createdByUserName;

            var updatedByUserName = (await _userAppService.FindByIdAsync(existing.UpdatedById)).DisplayName;
            result.CreatedByName = updatedByUserName;

            return result;
        }

        public async Task<AuthorizationPolicyResult> FindByNameAsync(string name)
        {
            var existing = await _authorizationPolicyRepository.FindByNameAsync(name);
            return MapEntityToDto(existing);
        }

        private AuthorizationPolicyResult MapEntityToDto(AuthorizationPolicy entity)
        {
            return new AuthorizationPolicyResult
            {
                CreatedById = entity.CreatedById,
                CreatedDate = entity.CreatedDate,
                Description = entity.Description,
                Id = entity.Id,
                Name = entity.Name,
                UpdatedById = entity.UpdatedById,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public async Task<BasePageList<AuthorizationPolicyResult>> GetAsync(AuthorizationPolicyFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = (from policy in _authorizationPolicyEntityRepository.Table
                         join createdBy in _userEntityRepository.Table
                         on policy.CreatedById equals createdBy.Id
                         join updatedBy in _userEntityRepository.Table
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

        public async Task<long> CreateAsync(AuthorizationPolicyRequest request)
        {
            var newPolicy = new AuthorizationPolicy
            {
                CreatedById = request.CreatedById,
                Description = request.Description,
                Name = request.Name,
                UpdatedById = request.UpdatedById,
            };
            return await _authorizationPolicyRepository.CreateAsync(newPolicy);
        }

        public async Task<bool> UpdateAsync(AuthorizationPolicyRequest request)
        {
            var existing = await _authorizationPolicyEntityRepository.FindAsync(x => x.Id == request.Id);

            existing.Description = request.Description;
            existing.Name = request.Name;
            existing.UpdatedById = request.UpdatedById;
            return await _authorizationPolicyRepository.UpdateAsync(existing);
        }

    }
}
