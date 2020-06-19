using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.IdentityDAL;

using System;
using System.Linq;

namespace Coco.Business.Implementation
{
    public class RoleAuthorizationPolicyBusiness : IRoleAuthorizationPolicyBusiness
    {
        private readonly IRepository<RoleAuthorizationPolicy> _roleAuthorizationPolicyRepository;
        private readonly IRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IRepository<Role> _roleRepository;
        //private readonly IdentityDbConnection _identityDbContext;

        public RoleAuthorizationPolicyBusiness(IRepository<RoleAuthorizationPolicy> roleAuthorizationPolicyRepository,
            IRepository<AuthorizationPolicy> authorizationPolicyRepository, IRepository<Role> userRepository)
        {
            _roleAuthorizationPolicyRepository = roleAuthorizationPolicyRepository;
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _roleRepository = userRepository;
            //_identityDbContext = identityDbContext;
        }

        public bool Add(byte roleId, short authorizationPolicyId, long loggedUserId)
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

            //_identityDbContext.SaveChanges();
            return true;
        }

        public bool Delete(byte roleId, short authorizationPolicyId)
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
            //_identityDbContext.SaveChanges();
            return true;
        }

        public AuthorizationPolicyRolesDto GetAuthoricationPolicyRoles(short id)
        {
            var authorizationUsers = _authorizationPolicyRepository.Get(x => x.Id == id)
                // TODO: include check
                //.Include(x => x.AuthorizationPolicyRoles)
                .Select(x => new AuthorizationPolicyRolesDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    AuthorizationPolicyRoles = x.AuthorizationPolicyRoles.Select(a => new RoleDto()
                    {
                        Id = a.RoleId,
                        Name = a.Role.Name,
                        Description = a.Role.Description
                    })
                })
                .FirstOrDefault();

            return authorizationUsers;
        }
    }
}
