using Coco.Business.Contracts;
using Coco.Data.Contracts;
using Coco.Business.Dtos.Identity;
using Coco.Data.Entities.Identity;
using System;
using System.Linq;

namespace Coco.Business.Implementation
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
