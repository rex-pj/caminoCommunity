using Camino.Business.Contracts;
using Camino.Business.AutoMap;
using Camino.Business.ValidationStrategies;
using Camino.Business.Dtos.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Data.Entities.Identity;
using System.Collections;
using System.Collections.Generic;

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
        public IEnumerable<UserRoleDto> GetUserRoles(long userd)
        {
            var userRoles = (from user in _userRepository.Table
                             join userRole in _userRoleRepository.Table
                             on user.Id equals userRole.UserId into roles
                             from userRole in roles.DefaultIfEmpty()
                             where user.Id == userd
                             select new UserRoleDto()
                             {
                                 UserId = user.Id,
                                 RoleId = userRole.RoleId,
                                 RoleName = userRole.Role.Name
                             }).ToList();

            return userRoles;
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
