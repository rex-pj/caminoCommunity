using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Users;
using Microsoft.EntityFrameworkCore;

namespace Camino.Application.AppServices.Authorization
{
    public class UserAuthorizationPolicyAppService : IUserAuthorizationPolicyAppService, IScopedDependency
    {
        private readonly IUserAuthorizationPolicyRepository _userAuthorizationPolicyRepository;
        private readonly IEntityRepository<UserAuthorizationPolicy> _userAuthorizationPolicyEntityRepository;
        private readonly IEntityRepository<AuthorizationPolicy> _authorizationPolicyEntityRepository;
        private readonly IEntityRepository<User> _userEntityRepository;

        public UserAuthorizationPolicyAppService(IEntityRepository<UserAuthorizationPolicy> userAuthorizationPolicyEntityRepository,
            IEntityRepository<AuthorizationPolicy> authorizationPolicyEntityRepository,
            IEntityRepository<User> userEntityRepository,
            IUserAuthorizationPolicyRepository userAuthorizationPolicyRepository)
        {
            _userAuthorizationPolicyRepository = userAuthorizationPolicyRepository;
            _authorizationPolicyEntityRepository = authorizationPolicyEntityRepository;
            _userEntityRepository = userEntityRepository;
            _userAuthorizationPolicyEntityRepository = userAuthorizationPolicyEntityRepository;
        }

        public async Task<bool> CreateAsync(long userId, long authorizationPolicyId, long loggedUserId)
        {
            if (userId <= 0 || authorizationPolicyId <= 0)
            {
                return false;
            }

            var isExist = _userAuthorizationPolicyEntityRepository
                .Get(x => x.UserId == userId && x.AuthorizationPolicyId == authorizationPolicyId).Any();
            if (isExist)
            {
                return false;
            }

            var userAuthorizationPolicy = new UserAuthorizationPolicy()
            {
                UserId = userId,
                GrantedById = loggedUserId,
                IsGranted = true,
                AuthorizationPolicyId = authorizationPolicyId
            };
            return await _userAuthorizationPolicyRepository.CreateAsync(userAuthorizationPolicy);
        }

        public async Task<bool> DeleteAsync(long userId, short authorizationPolicyId)
        {
            return await _userAuthorizationPolicyRepository.DeleteAsync(userId, authorizationPolicyId);
        }

        public AuthorizationPolicyUsersPageList GetAuthoricationPolicyUsers(long id, UserAuthorizationPolicyFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = from userAuthorization in _userAuthorizationPolicyEntityRepository.Get(x => x.AuthorizationPolicyId == id)
                        join user in _userEntityRepository.Table
                        on userAuthorization.UserId equals user.Id
                        where string.IsNullOrEmpty(search)
                        || user.Lastname.ToLower().Contains(search)
                        || user.Firstname.ToLower().Contains(search)
                        || (user.Lastname + " " + user.Firstname).ToLower().Contains(search)
                        select new UserResult
                        {
                            DisplayName = user.DisplayName,
                            Firstname = user.Firstname,
                            Lastname = user.Lastname,
                            Id = user.Id
                        };

            var filteredNumber = query.Select(x => x.Id).Count();
            var roles = query.Skip(filter.PageSize * (filter.Page - 1))
                            .Take(filter.PageSize).ToList();

            var authorizationPolicy = _authorizationPolicyEntityRepository.Get(x => x.Id == id)
                .Select(x => new AuthorizationPolicyUsersPageList
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .FirstOrDefault();
            authorizationPolicy.Collections = roles;
            authorizationPolicy.TotalResult = filteredNumber;
            authorizationPolicy.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);

            return authorizationPolicy;
        }

        public async Task<UserAuthorizationPolicyResult> GetUserAuthoricationPolicyAsync(long userId, long policyId)
        {
            var userAuthoricationPolicy = await _userAuthorizationPolicyEntityRepository
                .Get(x => x.UserId == userId && x.AuthorizationPolicyId == policyId)
                .Select(x => new UserAuthorizationPolicyResult
                {
                    AuthorizationPolicyId = x.AuthorizationPolicyId,
                    UserId = x.UserId
                })
                .FirstOrDefaultAsync();

            return userAuthoricationPolicy;
        }

        public async Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId)
        {
            return await _userAuthorizationPolicyRepository.IsUserHasAuthoricationPolicyAsync(userId, policyId);
        }
    }
}
