﻿using Coco.Business.Contracts;
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

            using(var transaction = _identityDbProvider.BeginTrsaction())
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
                catch(Exception e)
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
            if(user != null)
            {
                user.PasswordHash = model.NewPassword;
                await _userRepository.UpdateAsync(user);
            }            

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
