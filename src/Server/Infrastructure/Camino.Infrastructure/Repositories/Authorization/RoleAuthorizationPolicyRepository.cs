using Camino.Core.Contracts.Data;
using System;
using System.Linq;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.Authorization;

namespace Camino.Infrastructure.Repositories.Authorization
{
    public class RoleAuthorizationPolicyRepository : IRoleAuthorizationPolicyRepository
    {
        private readonly IRepository<RoleAuthorizationPolicy> _roleAuthorizationPolicyRepository;
        private readonly IRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IRepository<Role> _roleRepository;

        public RoleAuthorizationPolicyRepository(IRepository<RoleAuthorizationPolicy> roleAuthorizationPolicyRepository,
            IRepository<AuthorizationPolicy> authorizationPolicyRepository, IRepository<Role> userRepository)
        {
            _roleAuthorizationPolicyRepository = roleAuthorizationPolicyRepository;
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _roleRepository = userRepository;
        }

        public bool Create(long roleId, long authorizationPolicyId, long loggedUserId)
        {
            if (roleId <= 0 || authorizationPolicyId <= 0)
            {
                return false;
            }

            var isExist = _roleAuthorizationPolicyRepository.Get(x => x.RoleId == roleId && x.AuthorizationPolicyId == authorizationPolicyId)
                .Any();
            if (isExist)
            {
                return false;
            }

            _roleAuthorizationPolicyRepository.Add(new RoleAuthorizationPolicy()
            {
                RoleId = roleId,
                GrantedDate = DateTime.UtcNow,
                GrantedById = loggedUserId,
                IsGranted = true,
                AuthorizationPolicyId = authorizationPolicyId
            });

            return true;
        }

        public bool Delete(long roleId, long authorizationPolicyId)
        {
            var role = _roleRepository.FirstOrDefault(x => x.Id == roleId);
            if (role == null)
            {
                return false;
            }

            var authorizationPolicy = _authorizationPolicyRepository.FirstOrDefault(x => x.Id == authorizationPolicyId);
            if (authorizationPolicy == null)
            {
                return false;
            }

            var exist = _roleAuthorizationPolicyRepository.Get(x => x.RoleId == roleId && x.AuthorizationPolicyId == authorizationPolicyId);

            _roleAuthorizationPolicyRepository.Delete(exist);
            return true;
        }

        public AuthorizationPolicyRolesPageList GetAuthoricationPolicyRoles(long id, RoleAuthorizationPolicyFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";

            var query = from roleAuthorization in _roleAuthorizationPolicyRepository.Get(x => x.AuthorizationPolicyId == id)
                join role in _roleRepository.Table
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

            var authorizationPolicy = _authorizationPolicyRepository.Get(x => x.Id == id)
            .Select(x => new AuthorizationPolicyRolesPageList
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            })
            .FirstOrDefault();
            authorizationPolicy.Collections = roles;
            authorizationPolicy.TotalResult = filteredNumber;
            authorizationPolicy.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);

            return authorizationPolicy;
        }
    }
}
