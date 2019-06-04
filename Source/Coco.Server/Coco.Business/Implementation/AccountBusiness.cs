using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Account;
using Coco.Entities.Model.Account;
using Coco.UserDAL;
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

        public AccountBusiness(CocoUserDbContext dbContext, IRepository<UserInfo> userInfoRepository)
        {
            _dbContext = dbContext;
            _userInfoRepository = userInfoRepository;
        }

        public long Add(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            UserInfo userInfo = UserModelToEntity(userModel);

            _userInfoRepository.Insert(userInfo);
            _dbContext.SaveChanges();

            return userInfo.Id;
        }

        public async Task<UserModel> FindUserByEmail(string email, bool includeInActived = false)
        {
            email = email.ToLower();

            var user = await _userInfoRepository
                .Get(x => x.User.Email.Equals(email) && (includeInActived ? true : x.IsActived))
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            UserModel userModel = UserEntityToModel(user);

            return userModel;
        }

        public async Task<UserModel> FindUserByUsername(string username, bool includeInActived = false)
        {
            try
            {
                username = username.ToLower();

                var user = await _userInfoRepository
                    .Get(x => x.User.Email.Equals(username) && (includeInActived ? true : x.IsActived))
                    .Include(x => x.User)
                    .FirstOrDefaultAsync();

                UserModel userModel = UserEntityToModel(user);

                return userModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(long id)
        {
            var user = _userInfoRepository.Find(id);
            user.IsActived = false;

            _userInfoRepository.Update(user);
            _dbContext.SaveChanges();
        }

        public async Task<UserModel> UpdateAsync(UserModel user)
        {
            if (user.Id <= 0)
            {
                throw new ArgumentNullException("User Id");
            }

            UserInfo userInfo = _userInfoRepository.Find(user.Id);
            userInfo.Address = user.Address;
            userInfo.BirthDate = user.BirthDate;
            userInfo.CountryId = user.CountryId;
            userInfo.Description = user.Description;
            userInfo.GenderId = user.GenderId;
            userInfo.IsActived = user.IsActived;
            userInfo.PhoneNumber = user.PhoneNumber;
            userInfo.StatusId = user.StatusId;
            userInfo.UpdatedById = user.UpdatedById;
            userInfo.UpdatedDate = DateTime.Now;

            if (userInfo.User == null)
            {
                throw new ArgumentNullException(nameof(userInfo.User));
            }

            userInfo.User.DisplayName = user.DisplayName;
            userInfo.User.Firstname = user.Firstname;
            userInfo.User.Lastname = user.Lastname;
            userInfo.User.AuthenticatorToken = user.AuthenticatorToken;
            userInfo.User.SecurityStamp = user.SecurityStamp;
            userInfo.User.Expiration = user.Expiration;

            _userInfoRepository.Update(userInfo);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<UserModel> Find(long id)
        {
            var user = await _userInfoRepository
                .Get(x => x.Id == id && x.IsActived)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            var userModel = UserEntityToModel(user);

            return userModel;
        }

        public async Task<UserModel> FindByTokenAsync(long id, string authenticatorToken)
        {
            var existUser = await _userInfoRepository.Get(x => x.Id.Equals(id)).Include(x => x.User).FirstOrDefaultAsync();

            if (existUser != null && existUser.User.AuthenticatorToken.Equals(authenticatorToken))
            {
                var userModel = UserEntityToModel(existUser);

                return userModel;
            }

            return new UserModel();
        }

        #region Privates
        private UserInfo UserModelToEntity(UserModel userModel)
        {
            UserInfo user = new UserInfo
            {
                GenderId = userModel.GenderId,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                UpdatedById = userModel.UpdatedById,
                CreatedById = userModel.CreatedById,
                Address = userModel.Address,
                BirthDate = userModel.BirthDate,
                CountryId = userModel.CountryId,
                Description = userModel.Description,
                IsActived = false,
                PhoneNumber = userModel.PhoneNumber,
                StatusId = 1,
                User = new User()
                {
                    DisplayName = userModel.DisplayName,
                    Email = userModel.Email,
                    Firstname = userModel.Firstname,
                    Lastname = userModel.Lastname,
                    Password = userModel.Password,
                    PasswordSalt = userModel.PasswordSalt,
                    AuthenticatorToken = userModel.AuthenticatorToken,
                    SecurityStamp = userModel.SecurityStamp,
                    Expiration = userModel.Expiration
                }
            };

            return user;
        }

        private UserModel UserEntityToModel(UserInfo user)
        {
            if (user == null)
            {
                return null;
            }

            UserModel userModel = new UserModel
            {
                GenderId = user.GenderId,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate,
                UpdatedById = user.UpdatedById,
                CreatedById = user.CreatedById,
                Address = user.Address,
                BirthDate = user.BirthDate,
                CountryId = user.CountryId,
                Description = user.Description,
                IsActived = user.IsActived,
                PhoneNumber = user.PhoneNumber,
                StatusId = user.StatusId,
                Id = user.Id
            };

            if (user.User != null)
            {
                userModel.DisplayName = user.User.DisplayName;
                userModel.Email = user.User.Email;
                userModel.Firstname = user.User.Firstname;
                userModel.Lastname = user.User.Lastname;
                userModel.Password = user.User.Password;
                userModel.PasswordSalt = user.User.PasswordSalt;
                userModel.Expiration = user.User.Expiration;
                userModel.AuthenticatorToken = user.User.AuthenticatorToken;
                userModel.SecurityStamp = user.User.SecurityStamp;
            }

            return userModel;
        }
        #endregion
    }
}
