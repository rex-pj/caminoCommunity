using Coco.Business.Contracts;
using Coco.Business.Mapping;
using Coco.Business.ValidationStrategies;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Model.Account;
using Coco.Entities.Model.General;
using Coco.IdentityDAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation.UserBusiness
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IDbContext _identityContext;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        public AccountBusiness(IdentityDbContext identityContext, IRepository<User> userRepository,
            ValidationStrategyContext validationStrategyContext,
            IRepository<UserInfo> userInfoRepository)
        {
            _identityContext = identityContext;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }

        #region CRUD
        public long Add(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            userModel.StatusId = 1;
            userModel.IsActived = false;

            UserInfo userInfo = UserMapping.UserModelToEntity(userModel);

            _userInfoRepository.Insert(userInfo);
            _identityContext.SaveChanges();

            return userInfo.Id;
        }


        public void Delete(long id)
        {
            var user = _userRepository.Find(id);
            user.IsActived = false;

            _userRepository.Update(user);
            _identityContext.SaveChanges();
        }

        public async Task<UserModel> UpdateAsync(UserModel model)
        {
            if (model.Id <= 0)
            {
                throw new ArgumentNullException("User Id");
            }

            var user = await _userRepository.FindAsync(model.Id);

            user.IsActived = model.IsActived;
            user.StatusId = model.StatusId;
            user.UpdatedById = model.UpdatedById;
            user.UpdatedDate = DateTime.Now;
            user.AuthenticatorToken = model.AuthenticationToken;
            user.SecurityStamp = model.SecurityStamp;
            user.Expiration = model.Expiration;
            user.DisplayName = model.DisplayName;
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;

            if (user.UserInfo == null)
            {
                throw new ArgumentNullException(nameof(user.UserInfo));
            }

            user.UserInfo.Address = model.Address;
            user.UserInfo.BirthDate = model.BirthDate;
            user.UserInfo.CountryId = model.CountryId;
            user.UserInfo.Description = model.Description;
            user.UserInfo.GenderId = model.GenderId;
            user.UserInfo.PhoneNumber = model.PhoneNumber;

            _userRepository.Update(user);
            await _identityContext.SaveChangesAsync();

            return model;
        }

        public async Task<UpdatePerItem> UpdateInfoItemAsync(UpdatePerItem model)
        {
            if (model.PropertyName == null)
            {
                throw new ArgumentNullException(nameof(model.PropertyName));
            }

            if (model.Key == null)
            {
                throw new ArgumentNullException(nameof(model.Key));
            }

            var userInfo = _userInfoRepository.Find(model.Key);

            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }

            _validationStrategyContext.SetStrategy(new UserInfoItemUpdationValidationStratergy(_validationStrategyContext));
            bool canUpdate = _validationStrategyContext.Validate(model);

            if (!canUpdate)
            {
                throw new ArgumentException(model.PropertyName);
            }

            _userInfoRepository.UpdateByName(userInfo, model.Value, model.PropertyName, true);
            await _identityContext.SaveChangesAsync();

            return model;
        }
        #endregion

        public async Task<UserModel> FindUserByEmail(string email, bool includeInActived = false)
        {
            email = email.ToLower();

            var user = await _userRepository
                .GetAsNoTracking(x => x.Email.Equals(email) && (includeInActived ? true : x.IsActived))
                .Select(UserMapping.SelectorUserModel)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserModel> FindUserByUsername(string username, bool includeInActived = false)
        {
            try
            {
                username = username.ToLower();

                var user = await _userRepository
                    .GetAsNoTracking(x => x.Email.Equals(username) && (includeInActived ? true : x.IsActived))
                    .Select(UserMapping.SelectorUserModel)
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UserLoggedInModel GetLoggedIn(long id)
        {
            var user = _userRepository
                .GetAsNoTracking(x => x.Id == id)
                .Select(UserMapping.SelectorUserLoggedIn)
                .FirstOrDefault();

            return user;
        }

        public UserModel Find(long id)
        {
            var user = _userRepository
                .GetAsNoTracking(x => x.Id == id)
                .Select(UserMapping.SelectorUserModel)
                .FirstOrDefault();

            return user;
        }

        public async Task<UserModel> FindByIdAsync(long id)
        {
            var existUser = await _userRepository
                .GetAsNoTracking(x => x.Id.Equals(id))
                .Select(UserMapping.SelectorUserModel)
                .FirstOrDefaultAsync();

            return existUser;
        }

        public async Task<UserFullModel> GetFullByIdAsync(long id)
        {
            var existUser = await _userRepository
                .GetAsNoTracking(x => x.Id.Equals(id))
                .Select(UserMapping.SelectorFullUserModel)
                .FirstOrDefaultAsync();

            return existUser;
        }
    }
}
