using Camino.Business.Contracts;
using Camino.Data.Contracts;
using Camino.Business.Dtos.Identity;
using Camino.Data.Entities.Identity;
using System;
using System.Linq;
using AutoMapper;
using LinqToDB;
using System.Threading.Tasks;

namespace Camino.Business.Implementation
{
    public class UserAuthorizationPolicyBusiness : IUserAuthorizationPolicyBusiness
    {
        private readonly IRepository<UserAuthorizationPolicy> _userAuthorizationPolicyRepository;
        private readonly IRepository<RoleAuthorizationPolicy> _roleAuthorizationPolicyRepository;
        private readonly IRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserAuthorizationPolicyBusiness(IRepository<UserAuthorizationPolicy> userAuthorizationPolicyRepository,
            IRepository<AuthorizationPolicy> authorizationPolicyRepository, IRepository<User> userRepository,
            IRepository<UserRole> userRoleRepository, IRepository<Role> roleRepository, IMapper mapper,
            IRepository<RoleAuthorizationPolicy> roleAuthorizationPolicyRepository)
        {
            _userAuthorizationPolicyRepository = userAuthorizationPolicyRepository;
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _roleAuthorizationPolicyRepository = roleAuthorizationPolicyRepository;
        }

        public bool Add(long userId, short authorizationPolicyId, long loggedUserId)
        {
            if (userId <= 0 || authorizationPolicyId <= 0)
            {
                return false;
            }

            var isExist = _userAuthorizationPolicyRepository.Get(x => x.UserId == userId && x.AuthorizationPolicyId == authorizationPolicyId)
                .Any();
            if (isExist)
            {
                return false;
            }

            _userAuthorizationPolicyRepository.Add(new UserAuthorizationPolicy()
            {
                UserId = userId,
                GrantedDate = DateTime.UtcNow,
                GrantedById = loggedUserId,
                IsGranted = true,
                AuthorizationPolicyId = authorizationPolicyId
            });

            return true;
        }

        public bool Delete(long userId, short authorizationPolicyId)
        {
            var user = _userRepository.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return false;
            }

            var authorizationPolicy = _authorizationPolicyRepository.FirstOrDefault(x => x.Id == authorizationPolicyId);
            if (authorizationPolicy == null)
            {
                return false;
            }

            var exist = _userAuthorizationPolicyRepository.Get(x => x.UserId == userId && x.AuthorizationPolicyId == authorizationPolicyId);

            _userAuthorizationPolicyRepository.Delete(exist);
            return true;
        }

        public AuthorizationPolicyUsersDto GetAuthoricationPolicyUsers(short id)
        {
            var authorizationUsers = _authorizationPolicyRepository.Get(x => x.Id == id)
                // TODO: include check
                //.Include(x => x.AuthorizationPolicyUsers)
                .Select(x => new AuthorizationPolicyUsersDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    AuthorizationPolicyUsers = x.AuthorizationPolicyUsers.Select(a => new UserDto()
                    {
                        DisplayName = a.User.DisplayName,
                        Firstname = a.User.Firstname,
                        Lastname = a.User.Lastname,
                        Id = a.UserId
                    })
                })
                .FirstOrDefault();

            return authorizationUsers;
        }

        public async Task<UserAuthorizationPolicyDto> GetUserAuthoricationPolicyAsync(long userId, long policyId)
        {
            var userAuthoricationPolicy = await _userAuthorizationPolicyRepository
                .Get(x => x.UserId == userId && x.AuthorizationPolicyId == policyId)
                .FirstOrDefaultAsync();

            var userAuthorizationPolicyDto = _mapper.Map<UserAuthorizationPolicyDto>(userAuthoricationPolicy);
            return userAuthorizationPolicyDto;
        }

        public async Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId)
        {
            var isUserHasPolicy = await _userAuthorizationPolicyRepository
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
