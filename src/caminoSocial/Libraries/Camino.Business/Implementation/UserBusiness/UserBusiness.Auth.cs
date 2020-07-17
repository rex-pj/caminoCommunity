using Camino.Business.Contracts;
using Camino.Business.AutoMap;
using Camino.Business.ValidationStrategies;
using Camino.Business.Dtos.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Data.Entities.Identity;

namespace Camino.Business.Implementation.UserBusiness
{
    public partial class UserBusiness : IUserBusiness
    {
        #region CRUD
        public async Task<UserDto> CreateAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            userDto.StatusId = 1;
            userDto.IsActived = false;
            userDto.CreatedDate = DateTime.UtcNow;
            userDto.UpdatedDate = DateTime.UtcNow;

            var user = _mapper.Map<User>(userDto);
            var userInfo = _mapper.Map<UserInfo>(userDto);

            using (var transaction = _identityDbProvider.BeginTransaction())
            {
                try
                {
                    var userId = await _userRepository.AddWithInt64EntityAsync(user);
                    if (userId > 0)
                    {
                        userInfo.Id = userId;
                        await _userInfoRepository.AddWithInt64EntityAsync(userInfo);
                        transaction.Commit();
                        userDto.Id = userId;
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return userDto;
        }

        public async Task<UserDto> UpdatePasswordAsync(UserPasswordUpdateDto model)
        {
            _validationStrategyContext.SetStrategy(new UserPasswordUpdateValidationStratergy());
            bool canUpdate = _validationStrategyContext.Validate(model);
            if (!canUpdate)
            {
                throw new UnauthorizedAccessException();
            }

            var errors = _validationStrategyContext.Errors;
            if (errors != null || errors.Any())
            {
                throw new ArgumentException(_validationStrategyContext.Errors.FirstOrDefault().Message);
            }

            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if(user == null)
            {
                return new UserDto();
            }
            user.PasswordHash = model.NewPassword;
            await _userRepository.UpdateAsync(user);
            return new UserDto()
            {
                Id = user.Id
            };
        }
        #endregion

        #region GET
        public UserRoleAuthorizationPoliciesDto GetUserRolesAuthorizationPolicies(UserDto userDto)
        {
            var users = _userRepository.Get(x => x.Id == userDto.Id);
            var userAuthorizationPolicies = _userAuthorizationPolicyRepository.Get(x => x.UserId == userDto.Id);
            var userRoles = _userRoleRepository.Get(x => x.UserId == userDto.Id);

            var userRoleAuthorizationPolicy =
                (from user in users
                 join userAuthorizationPolicy in userAuthorizationPolicies
                 on user.Id equals userAuthorizationPolicy.UserId into userAuthorizations
                 from uap in userAuthorizations.DefaultIfEmpty()
                 join userRole in userRoles
                 on user.Id equals userRole.UserId into roles
                 from ur in roles.DefaultIfEmpty()
                 select new UserRoleAuthorizationPoliciesDto()
                 {
                     UserId = user.Id,
                     Firstname = user.Firstname,
                     Lastname = user.Lastname,
                     AuthorizationPolicies = userAuthorizations.Select(a => new AuthorizationPolicyDto()
                     {
                         Id = a.AuthorizationPolicyId,
                         Name = a.AuthorizationPolicy.Name,
                         Description = a.AuthorizationPolicy.Description
                     }),
                     Roles = userRoles.Select(r => new RoleAuthorizationPoliciesDto()
                     {
                         Id = r.RoleId,
                         Name = r.Role.Name,
                         AuthorizationPolicies = r.Role.RoleAuthorizationPolicies.Select(ra => new AuthorizationPolicyDto()
                         {
                             Id = ra.AuthorizationPolicyId,
                             Name = ra.AuthorizationPolicy.Name,
                             Description = ra.AuthorizationPolicy.Description
                         })
                     })
                 }).FirstOrDefault();

            return userRoleAuthorizationPolicy;
        }

        public UserDto GetLoggedIn(long id)
        {
            var user = _userRepository
                .Get(x => x.Id == id)
                .Select(UserExpressionMapping.UserModelSelector)
                .FirstOrDefault();

            return user;
        }
        #endregion
    }
}
