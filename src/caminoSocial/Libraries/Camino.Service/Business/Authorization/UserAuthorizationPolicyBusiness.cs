using Camino.Data.Contracts;
using Camino.Service.Projections.Identity;
using System;
using System.Linq;
using AutoMapper;
using LinqToDB;
using System.Threading.Tasks;
using Camino.Service.Projections.Filters;
using Camino.Service.Business.Authorization.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;

namespace Camino.Service.Business.Authorization
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

        public bool Add(long userId, long authorizationPolicyId, long loggedUserId)
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

        public AuthorizationPolicyUsersPageList GetAuthoricationPolicyUsers(long id, UserAuthorizationPolicyFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var authorizationPolicy = _authorizationPolicyRepository.Get(x => x.Id == id)
                .Select(x => new AuthorizationPolicyUsersPageList
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .FirstOrDefault(x => x.Id == id);

            var query = from userAuthorization in _userAuthorizationPolicyRepository.Get(x => x.AuthorizationPolicyId == id)
                        join user in _userRepository.Table
                        on userAuthorization.UserId equals user.Id
                        where string.IsNullOrEmpty(search) 
                        || user.Lastname.ToLower().Contains(search)
                        || user.Firstname.ToLower().Contains(search)
                        || (user.Lastname + " " + user.Firstname).ToLower().Contains(search)
                        select new UserProjection()
                        {
                            DisplayName = user.DisplayName,
                            Firstname = user.Firstname,
                            Lastname = user.Lastname,
                            Id = user.Id
                        };

            var filteredNumber = query.Select(x => x.Id).Count();
            var roles = query.Skip(filter.PageSize * (filter.Page - 1))
                            .Take(filter.PageSize).ToList();

            authorizationPolicy.Collections = roles;
            authorizationPolicy.TotalResult = filteredNumber;
            authorizationPolicy.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);

            return authorizationPolicy;
        }

        public async Task<UserAuthorizationPolicyProjection> GetUserAuthoricationPolicyAsync(long userId, long policyId)
        {
            var userAuthoricationPolicy = await _userAuthorizationPolicyRepository
                .Get(x => x.UserId == userId && x.AuthorizationPolicyId == policyId)
                .FirstOrDefaultAsync();

            var userAuthorizationPolicyRequest = _mapper.Map<UserAuthorizationPolicyProjection>(userAuthoricationPolicy);
            return userAuthorizationPolicyRequest;
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
