using Coco.Business.Contracts;
using Coco.Business.Mapping;
using Coco.Business.ValidationStrategies;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Model.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation.UserBusiness
{
    public partial class UserBusiness : IUserBusiness
    {
        #region CRUD
        public long Add(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            userModel.StatusId = 1;
            userModel.IsActived = false;

            var userInfo = _mapper.Map<UserInfo>(userModel);

            _userInfoRepository.Add(userInfo);
            _identityContext.SaveChanges();

            return userInfo.Id;
        }

        public async Task<UserModel> UpdateAuthenticationAsync(UserModel model)
        {
            if (model.Id <= 0)
            {
                throw new ArgumentNullException("User Id");
            }

            var user = await _userRepository.FindAsync(model.Id);

            user.UpdatedById = model.Id;
            user.UpdatedDate = DateTime.Now;
            user.AuthenticatorToken = model.AuthenticationToken;
            user.IdentityStamp = model.IdentityStamp;
            user.Expiration = model.Expiration;

            _userRepository.Update(user);
            await _identityContext.SaveChangesAsync();

            return model;
        }

        public async Task<UserLoggedInModel> UpdatePasswordAsync(UserPasswordUpdateModel model)
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

            return new UserLoggedInModel() {
                AuthenticationToken = user.AuthenticatorToken
            };
        }
        #endregion

        #region GET
        public UserLoggedInModel GetLoggedIn(long id)
        {
            var user = _userRepository
                .GetAsNoTracking(x => x.Id == id)
                .Select(UserMapping.SelectorUserLoggedIn)
                .FirstOrDefault();

            return user;
        }
        #endregion
    }
}
