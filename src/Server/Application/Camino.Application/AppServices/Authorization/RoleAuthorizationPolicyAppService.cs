using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Camino.Application.AppServices.Authorization
{
    public class RoleAuthorizationPolicyAppService : IRoleAuthorizationPolicyAppService, IScopedDependency
    {
        private readonly IRoleAuthorizationPolicyRepository _roleAuthorizationPolicyRepository;
        private readonly IEntityRepository<RoleAuthorizationPolicy> _roleAuthorizationPolicyEntityRepository;
        private readonly IEntityRepository<AuthorizationPolicy> _authorizationPolicyEntityRepository;
        private readonly IEntityRepository<Role> _roleEntityRepository;

        public RoleAuthorizationPolicyAppService(
            IEntityRepository<RoleAuthorizationPolicy> roleAuthorizationPolicyEntityRepository,
            IEntityRepository<AuthorizationPolicy> authorizationPolicyEntityRepository,
            IEntityRepository<Role> userEntityRepository,
            IRoleAuthorizationPolicyRepository roleAuthorizationPolicyRepository)
        {
            _authorizationPolicyEntityRepository = authorizationPolicyEntityRepository;
            _roleEntityRepository = userEntityRepository;

            _roleAuthorizationPolicyEntityRepository = roleAuthorizationPolicyEntityRepository;
            _roleAuthorizationPolicyRepository = roleAuthorizationPolicyRepository;
        }

        public async Task<bool> CreateAsync(long roleId, long authorizationPolicyId, long loggedUserId)
        {
            if (roleId <= 0 || authorizationPolicyId <= 0)
            {
                return false;
            }

            var isExist = (await _roleAuthorizationPolicyEntityRepository.GetAsync(x => x.RoleId == roleId && x.AuthorizationPolicyId == authorizationPolicyId)).Any();
            if (isExist)
            {
                return false;
            }
            return await _roleAuthorizationPolicyRepository.CreateAsynx(new RoleAuthorizationPolicy
            {
                RoleId = roleId,
                GrantedById = loggedUserId,
                IsGranted = true,
                AuthorizationPolicyId = authorizationPolicyId
            });
        }

        public async Task<bool> DeleteAsync(long roleId, long authorizationPolicyId)
        {
            return await _roleAuthorizationPolicyRepository.DeleteAsync(roleId, authorizationPolicyId);
        }

        public async Task<AuthorizationPolicyRolesPageList> GetPageListAsync(long id, RoleAuthorizationPolicyFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";

            var query = from roleAuthorization in _roleAuthorizationPolicyEntityRepository.Get(x => x.AuthorizationPolicyId == id)
                        join role in _roleEntityRepository.Table
                        on roleAuthorization.RoleId equals role.Id
                        where string.IsNullOrEmpty(search) || role.Name.ToLower().Contains(search)
                        || (role.Description != null && role.Description.ToLower().Contains(search))
                        select new RoleResult()
                        {
                            Id = role.Id,
                            Name = role.Name,
                            Description = role.Description
                        };

            var filteredNumber = query.Select(x => x.Id).Count();
            var roles = query.Skip(filter.PageSize * (filter.Page - 1))
                            .Take(filter.PageSize).ToList();

            var authorizationPolicy = await _authorizationPolicyEntityRepository.Get(x => x.Id == id)
            .Select(x => new AuthorizationPolicyRolesPageList
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            })
            .FirstOrDefaultAsync();
            
            authorizationPolicy.Collections = roles;
            authorizationPolicy.TotalResult = filteredNumber;
            authorizationPolicy.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);

            return authorizationPolicy;
        }
    }
}
