using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using Coco.IdentityDAL;

using System;
using System.Linq;

namespace Coco.Business.Implementation
{
    public class UserAuthorizationPolicyBusiness : IUserAuthorizationPolicyBusiness
    {
        private readonly IRepository<UserAuthorizationPolicy> _userAuthorizationPolicyRepository;
        private readonly IRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IRepository<User> _userRepository;
        //private readonly IdentityDbConnection _identityDbContext;

        public UserAuthorizationPolicyBusiness(IRepository<UserAuthorizationPolicy> userAuthorizationPolicyRepository,
            IRepository<AuthorizationPolicy> authorizationPolicyRepository, IRepository<User> userRepository)
        {
            _userAuthorizationPolicyRepository = userAuthorizationPolicyRepository;
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _userRepository = userRepository;
            //_identityDbContext = identityDbContext;
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

            //_identityDbContext.SaveChanges();
            return true;
        }

        public bool Delete(long userId, short authorizationPolicyId)
        {
            var user = _userRepository.Find(userId);
            if (user == null)
            {
                return false;
            }

            var authorizationPolicy = _authorizationPolicyRepository.Find(authorizationPolicyId);
            if (authorizationPolicy == null)
            {
                return false;
            }

            var exist = _userAuthorizationPolicyRepository.Get(x => x.UserId == userId && x.AuthorizationPolicyId == authorizationPolicyId);

            _userAuthorizationPolicyRepository.Delete(exist);
            //_identityDbContext.SaveChanges();
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
    }
}
