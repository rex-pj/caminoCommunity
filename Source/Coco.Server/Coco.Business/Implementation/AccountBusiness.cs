using Coco.Business.Contracts;
using Coco.Business.Mapping;
using Coco.Business.Validation;
using Coco.Business.Validation.Interfaces;
using Coco.Contract;
using Coco.Entities.Domain.Account;
using Coco.Entities.Model.Account;
using Coco.Entities.Model.General;
using Coco.UserDAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly CocoUserDbContext _dbContext;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<User> _userRepository;

        public AccountBusiness(CocoUserDbContext dbContext, IRepository<User> userRepository, IRepository<UserInfo> userInfoRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
        }

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
            _dbContext.SaveChanges();

            return userInfo.Id;
        }

        public async Task<UserModel> FindUserByEmail(string email, bool includeInActived = false)
        {
            email = email.ToLower();

            var user = await _userRepository
                .Get(x => x.Email.Equals(email) && (includeInActived ? true : x.IsActived))
                .Include(x => x.UserInfo)
                .FirstOrDefaultAsync();

            UserModel userModel = UserMapping.UserEntityToModel(user);

            return userModel;
        }

        public async Task<UserModel> FindUserByUsername(string username, bool includeInActived = false)
        {
            try
            {
                username = username.ToLower();

                var user = await _userRepository
                    .Get(x => x.Email.Equals(username) && (includeInActived ? true : x.IsActived))
                    .Include(x => x.UserInfo)
                    .FirstOrDefaultAsync();

                UserModel userModel = UserMapping.UserEntityToModel(user);

                return userModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(long id)
        {
            var user = _userRepository.Find(id);
            user.IsActived = false;

            _userRepository.Update(user);
            _dbContext.SaveChanges();
        }

        public async Task<UserModel> UpdateAsync(UserModel model)
        {
            if (model.Id <= 0)
            {
                throw new ArgumentNullException("User Id");
            }

            var user = _userRepository.Find(model.Id);

            user.IsActived = model.IsActived;
            user.StatusId = model.StatusId;
            user.UpdatedById = model.UpdatedById;
            user.UpdatedDate = DateTime.Now;
            user.AuthenticatorToken = model.AuthenticatorToken;
            user.SecurityStamp = model.SecurityStamp;
            user.Expiration = model.Expiration;


            user.UserInfo.DisplayName = model.DisplayName;
            user.UserInfo.Firstname = model.Firstname;
            user.UserInfo.Lastname = model.Lastname;

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
            await _dbContext.SaveChangesAsync();

            return model;
        }

        public async Task<UserModel> UpdateInfoAsync(UserModel user)
        {
            if (user.Id <= 0)
            {
                throw new ArgumentNullException("User Id");
            }

            UserInfo userInfo = _userInfoRepository.Find(user.Id);
            userInfo.BirthDate = user.BirthDate;
            userInfo.CountryId = user.CountryId;
            userInfo.Description = user.Description;
            userInfo.GenderId = user.GenderId;
            userInfo.PhoneNumber = user.PhoneNumber;
            userInfo.DisplayName = user.DisplayName;
            userInfo.Firstname = user.Firstname;
            userInfo.Lastname = user.Lastname;

            if (userInfo.User == null)
            {
                throw new ArgumentNullException(nameof(userInfo.User));
            }

            userInfo.User.UpdatedById = user.Id;
            userInfo.User.UpdatedDate = DateTime.Now;

            _userInfoRepository.Update(userInfo);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<UserModel> Find(long id)
        {
            var user = await _userRepository
                .Get(x => x.Id == id && x.IsActived)
                .Include(x => x.UserInfo)
                .FirstOrDefaultAsync();

            var userModel = UserMapping.UserEntityToModel(user);
            return userModel;
        }

        public async Task<UserModel> FindByIdAsync(long id)
        {
            var existUser = await _userRepository
                .Get(x => x.Id.Equals(id))
                .Include(x => x.UserInfo)
                .FirstOrDefaultAsync();

            if (existUser != null)
            {
                var userModel = UserMapping.UserEntityToModel(existUser);

                return userModel;
            }

            return new UserModel();
        }

        public async Task<UserFullModel> GetFullByIdAsync(long id)
        {
            var existUser = await _userRepository.Get(x => x.Id.Equals(id))
                .Include(x => x.Status)
                .Include(x => x.UserInfo)
                .Include(x => x.UserInfo.Country)
                .Include(x => x.UserInfo.Gender)
                .FirstOrDefaultAsync();

            if (existUser != null)
            {
                var userModel = UserMapping.FullUserEntityToModel(existUser);
                return userModel;
            }

            return new UserFullModel();
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

            bool canUpdate = ValidateInfoItem(model, userInfo);

            if (!canUpdate)
            {
                throw new ArgumentException(model.PropertyName);
            }

            _userInfoRepository.UpdateByName(userInfo, model.Value, model.PropertyName, true);
            await _dbContext.SaveChangesAsync();

            return model;
        }

        private bool ValidateInfoItem(UpdatePerItem model, UserInfo userInfo)
        {
            bool isValid = true;
            if (model.PropertyName.Equals(nameof(userInfo.Lastname),
                StringComparison.InvariantCultureIgnoreCase)
                || model.PropertyName.Equals(nameof(userInfo.Firstname),
                StringComparison.InvariantCultureIgnoreCase)
                || model.PropertyName.Equals(nameof(userInfo.DisplayName),
                StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.Value == null || string.IsNullOrEmpty(model.Value.ToString()))
                {
                    isValid = false;
                }
            }
            else if (model.PropertyName.Equals(nameof(userInfo.PhoneNumber),
                StringComparison.InvariantCultureIgnoreCase))
            {
                IValidation phoneValidation = new PhoneValidation();
                if (model.Value == null || string.IsNullOrEmpty(model.Value.ToString())
                    || !phoneValidation.IsValid(model.Value.ToString()))
                {
                    isValid = false;
                }
            }
            else if (model.PropertyName.Equals(nameof(userInfo.BirthDate),
                StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.Value == null)
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
