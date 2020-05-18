using Coco.Business.Contracts;
using Coco.Business.Mapping;
using Coco.Business.ValidationStrategies;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation.UserBusiness
{
    public partial class UserBusiness : IUserBusiness
    {
        #region CRUD
        public  async Task<UserDto> CreateAsync(UserDto userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            userModel.StatusId = 1;
            userModel.IsActived = false;
            userModel.CreatedDate = DateTime.UtcNow;
            userModel.UpdatedDate = DateTime.UtcNow;

            var userInfo = _mapper.Map<UserInfo>(userModel);

            _userInfoRepository.Add(userInfo);
            await _identityContext.SaveChangesAsync();
            userModel.Id = userInfo.Id;

            return userModel;
        }

        public async Task<UserDto> UpdatePasswordAsync(UserPasswordUpdateDto model)
        {
            _validationStrategyContext.SetStrategy(new UserPasswordUpdateValidationStratergy());
            bool canUpdate = _validationStrategyContext.Validate(model);
            if (!canUpdate)
            {
                foreach (var item in _validationStrategyContext.Errors)
                {
                    throw new ArgumentException(item.Message);
                }
            }

            var user = await _userRepository.FindAsync(model.UserId);

            user.PasswordHash = model.NewPassword;
            _userRepository.Update(user);
            await _identityContext.SaveChangesAsync();

            return new UserDto() {
                Id = user.Id
            };
        }

        public UserRoleAuthorizationPoliciesDto GetRoleAuthorizationPolicies(UserDto user)
        {
            var userRoleAuthorizationPolicy = _userRepository.Get(x => x.Id == user.Id)
                .Include(x => x.UserAuthorizationPolicies)
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
                .Select(UserMapping.UserModelSelector)
                .FirstOrDefault();

            return user;
        }
        #endregion
    }
}
