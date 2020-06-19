using Coco.Business.Contracts;
using Coco.Business.AutoMap;
using Coco.Business.ValidationStrategies;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation.UserBusiness
{
    public partial class UserBusiness : IUserBusiness
    {
        #region CRUD
        public async Task<UserDto> CreateAsync(UserDto user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.StatusId = 1;
            user.IsActived = false;
            user.CreatedDate = DateTime.UtcNow;
            user.UpdatedDate = DateTime.UtcNow;

            var userInfo = _mapper.Map<UserInfo>(user);

            await _userInfoRepository.AddAsync(userInfo);
            //await _identityContext.SaveChangesAsync();
            user.Id = userInfo.Id;

            return user;
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

            user.PasswordHash = model.NewPassword;
            _userRepository.Update(user);
            //await _identityContext.SaveChangesAsync();

            return new UserDto()
            {
                Id = user.Id
            };
        }

        public UserRoleAuthorizationPoliciesDto GetUserRolesAuthorizationPolicies(UserDto user)
        {
            var userRoleAuthorizationPolicy = _userRepository.Get(x => x.Id == user.Id)
                //.Include(x => x.UserAuthorizationPolicies)
                .Select(x => new UserRoleAuthorizationPoliciesDto()
                {
                    UserId = x.Id,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    AuthorizationPolicies = x.UserAuthorizationPolicies.Select(a => new AuthorizationPolicyDto()
                    {
                        Id = a.AuthorizationPolicyId,
                        Name = a.AuthorizationPolicy.Name,
                        Description = a.AuthorizationPolicy.Description
                    }),
                    Roles = x.UserRoles.Select(r => new RoleAuthorizationPoliciesDto()
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
                })
                .FirstOrDefault();

            return userRoleAuthorizationPolicy;
        }
        #endregion

        #region GET
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
