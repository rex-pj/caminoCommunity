using Camino.Business.Contracts;
using Camino.Business.AutoMap;
using Camino.Business.ValidationStrategies;
using Camino.Data.Contracts;
using Camino.Business.Dtos.Identity;
using Camino.Business.Dtos.General;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Camino.Data.Enums;
using System.Collections.Generic;
using LinqToDB;
using Camino.IdentityDAL.Contracts;
using Camino.Data.Entities.Identity;

namespace Camino.Business.Implementation.UserBusiness
{
    public partial class UserBusiness : IUserBusiness
    {
        #region Fields/Properties
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        private readonly IMapper _mapper;
        private readonly IIdentityDataProvider _identityDbProvider;
        #endregion

        #region Ctor
        public UserBusiness(IRepository<User> userRepository,
            ValidationStrategyContext validationStrategyContext,
            IMapper mapper,
            IRepository<UserInfo> userInfoRepository,
            IIdentityDataProvider identityDbProvider)
        {
            _identityDbProvider = identityDbProvider;
            _mapper = mapper;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }
        #endregion

        #region CRUD
        public async Task<UserDto> CreateAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            userDto.StatusId = 1;
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

        public async Task DeleteAsync(long id)
        {
            var user = _userRepository.FirstOrDefault(x => x.Id == id);
            user.StatusId = (int)UserStatus.Deleted;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ActiveAsync(long id)
        {
            var user = _userRepository.FirstOrDefault(x => x.Id == id);
            if (user.IsEmailConfirmed)
            {
                throw new InvalidOperationException($"User with email: {user.Email} is already actived");
            }

            user.IsEmailConfirmed = true;
            user.StatusId = (int)UserStatus.Actived;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<UpdatePerItem> UpdateInfoItemAsync(UpdatePerItem model)
        {
            if (model.PropertyName == null)
            {
                throw new ArgumentException(nameof(model.PropertyName));
            }

            if (model.Key == null)
            {
                throw new ArgumentException(nameof(model.Key));
            }

            var key = (long)model.Key;
            var userInfo = _userInfoRepository.FirstOrDefault(x => x.Id == key);

            if (userInfo == null)
            {
                throw new ArgumentException(nameof(userInfo));
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

            await _identityDbProvider.UpdateByNameAsync(userInfo, model.Value, model.PropertyName, true);

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

            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == model.Id);

            user.UpdatedById = model.Id;
            user.UpdatedDate = DateTime.UtcNow;
            user.Lastname = model.Lastname;
            user.Firstname = model.Firstname;
            user.DisplayName = model.DisplayName;

            _userRepository.Update(user);

            return model;
        }

        public async Task<UserDto> UpdateAsync(UserDto user)
        {
            var exist = await _userRepository.FirstOrDefaultAsync(x => x.Id == user.Id);

            exist.UpdatedById = user.Id;
            exist.UpdatedDate = DateTime.UtcNow;
            exist.Lastname = user.Lastname;
            exist.Firstname = user.Firstname;
            exist.DisplayName = user.DisplayName;
            exist.IsEmailConfirmed = user.IsEmailConfirmed;
            exist.PasswordHash = user.PasswordHash;

            _userRepository.Update(exist);

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

        public List<UserFullDto> Search(string query = "", List<long> currentUserIds = null, int page = 1, int pageSize = 10)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            query = query.ToLower();

            var hasCurrentUserIds = currentUserIds != null && currentUserIds.Any();
            var data = _userRepository.Get(x => string.IsNullOrEmpty(query) || x.Lastname.ToLower().Contains(query)
                || x.Firstname.ToLower().Contains(query) || x.DisplayName.ToLower().Contains(query))
                .Where(x => !hasCurrentUserIds || !currentUserIds.Contains(x.Id));

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

        public async Task<PageListDto<UserFullDto>> GetAsync(UserFilterDto filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var userQuery = _userRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                userQuery = userQuery.Where(user => user.Lastname.ToLower().Contains(search)
                         || user.Firstname.ToLower().Contains(search)
                         || (user.Lastname + " " + user.Firstname).ToLower().Contains(search)
                         || user.Email.Contains(search)
                         || user.DisplayName.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                userQuery = userQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                userQuery = userQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.StatusId.HasValue)
            {
                userQuery = userQuery.Where(x => x.StatusId == filter.StatusId);
            }

            if (filter.IsEmailConfirmed.HasValue)
            {
                userQuery = userQuery.Where(x => x.IsEmailConfirmed == filter.IsEmailConfirmed);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                userQuery = userQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                userQuery = userQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                userQuery = userQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            // Filter in UserInfo
            var userInfoQuery = _userInfoRepository.Table;
            if (filter.GenderId.HasValue)
            {
                userInfoQuery = userInfoQuery.Where(x => x.GenderId == filter.GenderId);
            }

            if (filter.CountryId.HasValue)
            {
                userInfoQuery = userInfoQuery.Where(x => x.CountryId == filter.CountryId);
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                userInfoQuery = userInfoQuery.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                userInfoQuery = userInfoQuery.Where(x => x.Address.Contains(filter.Address));
            }

            // Filter by birthdate
            if (filter.BirthDateFrom.HasValue && filter.BirthDateTo.HasValue)
            {
                userInfoQuery = userInfoQuery.Where(x => x.BirthDate >= filter.BirthDateFrom && x.BirthDate <= filter.BirthDateTo);
            }
            else if (filter.BirthDateTo.HasValue)
            {
                userInfoQuery = userInfoQuery.Where(x => x.BirthDate <= filter.BirthDateTo);
            }
            else if (filter.BirthDateFrom.HasValue)
            {
                userInfoQuery = userInfoQuery.Where(x => x.BirthDate >= filter.BirthDateFrom && x.BirthDate <= DateTime.UtcNow);
            }

            var query = (from user in userQuery
                         join userInfo in userInfoQuery
                         on user.Id equals userInfo.Id
                         select new UserFullDto()
                         {
                             Id = user.Id,
                             Email = user.Email,
                             Address = user.UserInfo.Address,
                             Lastname = user.Lastname,
                             Firstname = user.Firstname,
                             DisplayName = user.DisplayName,
                             CreatedDate = user.CreatedDate,
                             UpdatedDate = user.UpdatedDate,
                             BirthDate = user.UserInfo.BirthDate,
                             IsEmailConfirmed = user.IsEmailConfirmed,
                             PhoneNumber = user.UserInfo.PhoneNumber,
                             GenderLabel = user.UserInfo.Gender.Name,
                             StatusLabel = user.Status.Name,
                             CountryName = user.UserInfo.Country.Name
                         });

            var filteredNumber = query.Select(x => x.Id).Count();

            var users = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize)
                                         .ToListAsync();

            var result = new PageListDto<UserFullDto>(users);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }
        #endregion
    }
}
