using Coco.Business.Contracts;
using Coco.Business.Mapping;
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

            UserInfo userInfo = UserMapping.UserModelToEntity(userModel);

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

            UserModel userModel = UserMapping.UserEntityToModel(user);

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

            var userModel = UserMapping.UserEntityToModel(user);

            return userModel;
        }

        public async Task<UserModel> FindByTokenAsync(long id, string authenticatorToken)
        {
            var existUser = await _userInfoRepository.Get(x => x.Id.Equals(id)).Include(x => x.User).FirstOrDefaultAsync();

            if (existUser != null && existUser.User.AuthenticatorToken.Equals(authenticatorToken))
            {
                var userModel = UserMapping.UserEntityToModel(existUser);

                return userModel;
            }

            return new UserModel();
        }

        public async Task<UserFullModel> GetFullByTokenAsync(long id, string authenticatorToken)
        {
            var existUser = await _userInfoRepository.Get(x => x.Id.Equals(id))
                .Include(x => x.Country)
                .Include(x => x.Gender)
                .Include(x => x.User).FirstOrDefaultAsync();

            if (existUser != null && existUser.User.AuthenticatorToken.Equals(authenticatorToken))
            {
                var userModel = UserMapping.FullUserEntityToModel(existUser);

                return userModel;
            }

            return new UserFullModel();
        }
    }
}
