using Coco.Business.Contracts;
using Coco.Business.Mapping;
using Coco.Business.ValidationStrategies;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.User;
using Coco.Entities.Dtos.General;
using Coco.IdentityDAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Coco.Entities.Enums;
using System.Collections.Generic;

namespace Coco.Business.Implementation.UserBusiness
{
    public partial class UserBusiness : IUserBusiness
    {
        #region Fields/Properties
        private readonly IDbContext _identityContext;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public UserBusiness(IdentityDbContext identityContext, IRepository<User> userRepository,
            ValidationStrategyContext validationStrategyContext,
            IMapper mapper,
            IRepository<UserInfo> userInfoRepository)
        {
            _identityContext = identityContext;
            _mapper = mapper;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }
        #endregion

        #region CRUD
        public async Task DeleteAsync(long id)
        {
            var user = _userRepository.Find(id);
            user.IsActived = false;

            _userRepository.Update(user);
            await _identityContext.SaveChangesAsync();
        }

        public async Task<bool> ActiveAsync(long id)
        {
            var user = _userRepository.Find(id);
            if (user.IsActived)
            {
                throw new InvalidOperationException($"User with email: {user.Email} is already actived");
            }

            user.IsActived = true;
            user.IsEmailConfirmed = true;
            user.StatusId = (byte)UserStatusEnum.Actived;
            _userRepository.Update(user);
            await _identityContext.SaveChangesAsync();

            return true;
        }

        public async Task<UpdatePerItemDto> UpdateInfoItemAsync(UpdatePerItemDto model)
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

            if (userInfo.User != null)
            {
                userInfo.User.UpdatedDate = DateTime.UtcNow;
                userInfo.User.UpdatedById = userInfo.Id;
            }
            _userInfoRepository.UpdateByName(userInfo, model.Value, model.PropertyName, true);
            await _identityContext.SaveChangesAsync();

            return model;
        }

        public async Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(UserIdentifierUpdateDto model)
        {
            _validationStrategyContext.SetStrategy(new UserProfileUpdateValidationStratergy());
            bool canUpdate = _validationStrategyContext.Validate(model);
            if (!canUpdate)
            {
                foreach (var item in _validationStrategyContext.Errors)
                {
                    throw new ArgumentNullException(item.Message);
                }
            }

            var user = await _userRepository.FindAsync(model.Id);

            user.UpdatedById = model.Id;
            user.UpdatedDate = DateTime.UtcNow;
            user.Lastname = model.Lastname;
            user.Firstname = model.Firstname;
            user.DisplayName = model.DisplayName;

            _userRepository.Update(user);
            await _identityContext.SaveChangesAsync();

            return model;
        }

        public async Task<UserDto> UpdateAsync(UserDto user)
        {
            var exist = await _userRepository.FindAsync(user.Id);

            exist.UpdatedById = user.Id;
            exist.UpdatedDate = DateTime.UtcNow;
            exist.Lastname = user.Lastname;
            exist.Firstname = user.Firstname;
            exist.DisplayName = user.DisplayName;

            _userRepository.Update(exist);
            await _identityContext.SaveChangesAsync();

            return user;
        }
        #endregion

        #region GET
        public async Task<UserDto> FindByEmailAsync(string email)
        {
            email = email.ToLower();

            var user = await _userRepository
                .Get(x => x.Email.Equals(email))
                .Select(UserExpressionMapping.UserModelSelector)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserDto> FindByUsernameAsync(string username)
        {
            username = username.ToLower();

            var user = await _userRepository
                .Get(x => x.Email.Equals(username))
                .Select(UserExpressionMapping.UserModelSelector)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserDto> FindByIdAsync(long id)
        {
            var existUser = await _userRepository
                .Get(x => x.Id.Equals(id))
                .Select(UserExpressionMapping.UserModelSelector)
                .FirstOrDefaultAsync();

            return existUser;
        }

        public async Task<UserFullDto> FindFullByIdAsync(long id)
        {
            var existUser = await _userRepository
                .Get(x => x.Id.Equals(id))
                .Select(UserExpressionMapping.FullUserModelSelector)
                .FirstOrDefaultAsync();

            return existUser;
        }

        public List<UserFullDto> Search(string query = "", int page = 1, int pageSize = 10)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            query = query.ToLower();

            var data = _userRepository.Get(x => string.IsNullOrEmpty(query) || x.Lastname.ToLower().Contains(query)
                || x.Firstname.ToLower().Contains(query) || x.DisplayName.ToLower().Contains(query));

            data = data.Skip(0).Take(10);
            if (pageSize > 0)
            {
                data = data.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var users = data
                .Select(x => new UserFullDto()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Lastname = x.Lastname,
                    Firstname = x.Firstname,
                    DisplayName = x.DisplayName
                })
                .ToList();

            return users;
        }

        public List<UserFullDto> GetFull()
        {
            var users = _userRepository.Get()
                .Select(x => new UserFullDto()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Address = x.UserInfo.Address,
                    AvatarUrl = x.UserInfo.AvatarUrl,
                    Lastname = x.Lastname,
                    Firstname = x.Firstname,
                    BirthDate = x.UserInfo.BirthDate,
                    IsEmailConfirmed = x.IsEmailConfirmed,
                    PhoneNumber = x.UserInfo.PhoneNumber,
                    GenderLabel = x.UserInfo.Gender.Name,
                    IsActived = x.IsActived,
                    StatusLabel = x.Status.Name,
                    CountryName = x.UserInfo.Country.Name
                })
                .ToList();

            return users;
        }
        #endregion
    }
}
