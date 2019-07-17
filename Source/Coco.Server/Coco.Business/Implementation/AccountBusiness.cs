using Coco.Business.Contracts;
using Coco.Business.Mapping;
using Coco.Business.ValidationStrategies;
using Coco.Common.Helper;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Model.Account;
using Coco.Entities.Model.General;
using Coco.IdentityDAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly CocoUserDbContext _dbContext;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        public AccountBusiness(CocoUserDbContext dbContext, IRepository<User> userRepository,
            ValidationStrategyContext validationStrategyContext,
            IRepository<UserInfo> userInfoRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
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

            if (userInfo.User == null)
            {
                throw new ArgumentNullException(nameof(userInfo.User));
            }

            userInfo.User.DisplayName = user.DisplayName;
            userInfo.User.Firstname = user.Firstname;
            userInfo.User.Lastname = user.Lastname;
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

        public UserModel FindByIdAsync(long id)
        {
            var existUser = _userRepository
                .Get(x => x.Id.Equals(id))
                .Include(x => x.UserInfo)
                .FirstOrDefault();

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
            await _dbContext.SaveChangesAsync();

            return model;
        }

        public async Task<UpdateAvatarModel> UpdatePhotoAsync(UpdateAvatarModel model, long userId)
        {
            var userInfo = _userInfoRepository.Find(userId);

            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }

            _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
            bool canUpdate = _validationStrategyContext.Validate(model.PhotoUrl);

            if (!canUpdate)
            {
                throw new ArgumentException(model.PhotoUrl);
            }

            _validationStrategyContext.SetStrategy(new AvatarValidationStrategy());
            canUpdate = _validationStrategyContext.Validate(model);

            if (!canUpdate)
            {
                throw new ArgumentException("Avatar Url");
            }

            var newImage = ImageHelper
                .CropBase64Image(model.PhotoUrl, model.XAxis, model.YAxis, model.Width, model.Height);

            userInfo.Photo = newImage;
            _userInfoRepository.Update(userInfo);
            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
