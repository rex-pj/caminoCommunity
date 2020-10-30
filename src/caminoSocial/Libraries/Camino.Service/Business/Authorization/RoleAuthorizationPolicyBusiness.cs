using Camino.Data.Contracts;
using Camino.Service.Projections.Identity;
using System;
using System.Linq;
using Camino.Service.Projections.Filters;
using Camino.Service.Business.Authorization.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;

namespace Camino.Service.Business.Authorization
{
    public class RoleAuthorizationPolicyBusiness : IRoleAuthorizationPolicyBusiness
    {
        private readonly IRepository<RoleAuthorizationPolicy> _roleAuthorizationPolicyRepository;
        private readonly IRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IRepository<Role> _roleRepository;

        public RoleAuthorizationPolicyBusiness(IRepository<RoleAuthorizationPolicy> roleAuthorizationPolicyRepository,
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
            var authorizationPolicy = _authorizationPolicyRepository.Get(x => x.Id == id)
                .Select(x => new AuthorizationPolicyRolesPageList
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .FirstOrDefault(x => x.Id == id);

            var query = from roleAuthorization in _roleAuthorizationPolicyRepository.Get(x => x.AuthorizationPolicyId == id)
                join role in _roleRepository.Table
                on roleAuthorization.RoleId equals role.Id
                where string.IsNullOrEmpty(search) || role.Name.ToLower().Contains(search)
                || (role.Description != null && role.Description.ToLower().Contains(search))
                select new RoleProjection()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                };

            var filteredNumber = query.Select(x => x.Id).Count();
            var roles = query.Skip(filter.PageSize * (filter.Page - 1))
                            .Take(filter.PageSize).ToList();

            authorizationPolicy.Collections = roles;
            authorizationPolicy.TotalResult = filteredNumber;
            authorizationPolicy.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);

            return authorizationPolicy;
        }
    }
}
