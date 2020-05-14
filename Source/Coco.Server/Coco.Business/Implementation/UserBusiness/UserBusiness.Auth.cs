using Coco.Business.Contracts;
using Coco.Business.Mapping;
using Coco.Business.ValidationStrategies;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.User;
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

            user.Password = model.NewPassword;
            _userRepository.Update(user);
            await _identityContext.SaveChangesAsync();

            return new UserDto() {
                Id = user.Id
            };
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
