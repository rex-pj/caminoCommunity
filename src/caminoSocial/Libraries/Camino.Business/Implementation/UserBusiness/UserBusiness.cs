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
        private readonly IRepository<UserAuthorizationPolicy> _userAuthorizationPolicyRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        private readonly IMapper _mapper;
        private readonly IIdentityDataProvider _identityDbProvider;
        #endregion

        #region Ctor
        public UserBusiness(IRepository<User> userRepository,
            ValidationStrategyContext validationStrategyContext,
            IMapper mapper,
            IRepository<UserAuthorizationPolicy> userAuthorizationPolicyRepository,
            IRepository<UserInfo> userInfoRepository,
            IRepository<UserRole> userRoleRepository,
            IIdentityDataProvider identityDbProvider)
        {
            _identityDbProvider = identityDbProvider;
            _mapper = mapper;
            _userAuthorizationPolicyRepository = userAuthorizationPolicyRepository;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _userRoleRepository = userRoleRepository;
            _validationStrategyContext = validationStrategyContext;
        }
        #endregion

        #region CRUD
        public async Task DeleteAsync(long id)
        {
            var user = _userRepository.FirstOrDefault(x => x.Id == id);
            user.IsActived = false;

            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ActiveAsync(long id)
        {
            var user = _userRepository.FirstOrDefault(x => x.Id == id);
            if (user.IsActived)
            {
                throw new InvalidOperationException($"User with email: {user.Email} is already actived");
            }

            user.IsActived = true;
            user.IsEmailConfirmed = true;
            user.StatusId = (byte)UserStatus.Actived;
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
            var query = (from user in _userRepository.Table
                         join userInfo in _userInfoRepository.Table
                         on user.Id equals userInfo.Id
                         where (string.IsNullOrEmpty(search)
                         || user.Lastname.ToLower().Contains(search)
                         || user.Firstname.ToLower().Contains(search)
                         || (user.Lastname + " " + user.Firstname).ToLower().Contains(search)
                         || user.Email.Contains(search)
                         || user.DisplayName.ToLower().Contains(search))

                         && (!filter.CreatedById.HasValue || user.CreatedById == filter.CreatedById)
                         && (!filter.UpdatedById.HasValue || user.UpdatedById == filter.UpdatedById)
                         && (!filter.StatusId.HasValue || user.StatusId == filter.StatusId)
                         && (!filter.IsActived.HasValue || user.IsActived == filter.IsActived)

                         && (!filter.GenderId.HasValue || userInfo.GenderId == filter.GenderId)
                         && (!filter.CountryId.HasValue || userInfo.CountryId == filter.CountryId)
                         && (string.IsNullOrEmpty(filter.PhoneNumber) || userInfo.PhoneNumber.Contains(filter.PhoneNumber))
                         && (string.IsNullOrEmpty(filter.Address) || userInfo.Address.Contains(filter.Address))
                         select new UserFullDto()
                         {
                             Id = user.Id,
                             Email = user.Email,
                             Address = user.UserInfo.Address,
                             Lastname = user.Lastname,
                             Firstname = user.Firstname,
                             CreatedDate = user.CreatedDate,
                             UpdatedDate = user.UpdatedDate,
                             BirthDate = user.UserInfo.BirthDate,
                             IsEmailConfirmed = user.IsEmailConfirmed,
                             PhoneNumber = user.UserInfo.PhoneNumber,
                             GenderLabel = user.UserInfo.Gender.Name,
                             IsActived = user.IsActived,
                             StatusLabel = user.Status.Name,
                             CountryName = user.UserInfo.Country.Name
                         });

            // Filter by birthdate
            if (filter.BirthDateFrom.HasValue && filter.BirthDateTo.HasValue)
            {
                query = query.Where(x => x.BirthDate >= filter.BirthDateFrom && x.BirthDate <= filter.BirthDateTo);
            }
            else if (filter.BirthDateTo.HasValue)
            {
                query = query.Where(x => x.BirthDate <= filter.BirthDateTo);
            }
            else if (filter.BirthDateFrom.HasValue)
            {
                query = query.Where(x => x.BirthDate >= filter.BirthDateFrom && x.BirthDate <= DateTime.UtcNow);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            // Filter by updated date
            if (filter.UpdatedDateFrom.HasValue && filter.UpdatedDateTo.HasValue)
            {
                query = query.Where(x => x.UpdatedDate >= filter.UpdatedDateFrom && x.UpdatedDate <= filter.UpdatedDateTo);
            }
            else if (filter.UpdatedDateTo.HasValue)
            {
                query = query.Where(x => x.UpdatedDate <= filter.UpdatedDateTo);
            }
            else if (filter.UpdatedDateFrom.HasValue)
            {
                query = query.Where(x => x.UpdatedDate >= filter.UpdatedDateFrom && x.UpdatedDate <= DateTime.UtcNow);
            }

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
