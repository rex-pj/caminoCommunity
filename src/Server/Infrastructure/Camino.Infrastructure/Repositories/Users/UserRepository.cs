using Camino.Core.Contracts.Data;
using Camino.Shared.Results.Identifiers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Enums;
using System.Collections.Generic;
using LinqToDB;
using Camino.Shared.Requests.Filters;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Shared.Requests.UpdateItems;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Requests.Identifiers;
using Camino.Infrastructure.Strategies.Validations;
using Camino.Infrastructure.Data;
using LinqToDB.Tools;

namespace Camino.Infrastructure.Repositories.Users
{
    public partial class UserRepository : IUserRepository
    {
        #region Fields/Properties
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        private readonly CaminoDataConnection _dataConnection;
        #endregion

        #region Ctor
        public UserRepository(IRepository<User> userRepository, ValidationStrategyContext validationStrategyContext,
            IRepository<UserInfo> userInfoRepository, CaminoDataConnection dataConnection)
        {
            _dataConnection = dataConnection;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(UserModifyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = new User
            {
                CreatedById = request.CreatedById,
                CreatedDate = DateTime.UtcNow,
                UpdatedById = request.UpdatedById,
                UpdatedDate = DateTime.UtcNow,
                Email = request.Email,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                PasswordHash = request.PasswordHash,
                SecurityStamp = request.SecurityStamp,
                StatusId = 1,
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                IsEmailConfirmed = true
            };

            using (var transaction = _dataConnection.BeginTransaction())
            {
                try
                {
                    var userId = await _userRepository.AddWithInt64EntityAsync(user);
                    if (userId > 0)
                    {
                        await _userInfoRepository.AddWithInt64EntityAsync(new UserInfo
                        {
                            BirthDate = request.BirthDate,
                            Description = request.Description,
                            GenderId = request.GenderId,
                            Id = userId
                        });
                        transaction.Commit();

                        return userId;
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

            return -1;
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

        public async Task<UpdateItemRequest> UpdateInfoItemAsync(UpdateItemRequest request)
        {
            if (request.PropertyName == null)
            {
                throw new ArgumentException(nameof(request.PropertyName));
            }

            if (request.Key == null)
            {
                throw new ArgumentException(nameof(request.Key));
            }

            var key = (long)request.Key;
            var userInfo = _userInfoRepository.FirstOrDefault(x => x.Id == key);

            if (userInfo == null)
            {
                throw new ArgumentException(nameof(userInfo));
            }

            _validationStrategyContext.SetStrategy(new UserInfoItemUpdationValidationStratergy(_validationStrategyContext));
            bool canUpdate = _validationStrategyContext.Validate(request);

            if (!canUpdate)
            {
                throw new ArgumentException(request.PropertyName);
            }

            if (userInfo.User != null)
            {
                userInfo.User.UpdatedDate = DateTime.UtcNow;
                userInfo.User.UpdatedById = userInfo.Id;
            }

            await _dataConnection.UpdateByNameAsync(userInfo, request.Value, request.PropertyName, true);

            return request;
        }

        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(UserIdentifierUpdateRequest request)
        {
            _validationStrategyContext.SetStrategy(new UserProfileUpdateValidationStratergy());
            bool canUpdate = _validationStrategyContext.Validate(request);
            if (!canUpdate)
            {
                foreach (var item in _validationStrategyContext.Errors)
                {
                    throw new ArgumentNullException(item.Message);
                }
            }

            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == request.Id);

            user.UpdatedById = request.Id;
            user.UpdatedDate = DateTime.UtcNow;
            user.Lastname = request.Lastname;
            user.Firstname = request.Firstname;
            user.DisplayName = request.DisplayName;

            _userRepository.Update(user);

            return request;
        }

        public async Task<bool> UpdateAsync(UserModifyRequest request)
        {
            var exist = await _userRepository.Get(x => x.Id == request.Id)
                .Set(x => x.UpdatedById, request.Id)
                .Set(x => x.UpdatedDate, DateTime.UtcNow)
                .Set(x => x.Lastname, request.Lastname)
                .Set(x => x.Firstname, request.Firstname)
                .Set(x => x.DisplayName, request.DisplayName)
                .Set(x => x.IsEmailConfirmed, request.IsEmailConfirmed)
                .Set(x => x.PasswordHash, request.PasswordHash)
                .UpdateAsync();

            return true;
        }
        #endregion

        #region GET
        public async Task<UserResult> FindByEmailAsync(string email)
        {
            email = email.ToLower();

            var user = await _userRepository
                .Get(x => x.Email.Equals(email))
                .Select(x => new UserResult
                {
                    DisplayName = x.DisplayName,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    UserName = x.UserName,
                    UpdatedDate = x.UpdatedDate,
                    CreatedDate = x.CreatedDate,
                    UpdatedById = x.UpdatedById,
                    CreatedById = x.CreatedById,
                    StatusId = x.StatusId,
                    Email = x.Email,
                    PasswordHash = x.PasswordHash,
                    SecurityStamp = x.SecurityStamp,
                    Id = x.Id,
                    IsEmailConfirmed = x.IsEmailConfirmed,
                    GenderId = x.UserInfo.GenderId,
                    Address = x.UserInfo.Address,
                    BirthDate = x.UserInfo.BirthDate,
                    CountryId = x.UserInfo.CountryId,
                    PhoneNumber = x.UserInfo.PhoneNumber,
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserResult> FindByUsernameAsync(string username)
        {
            username = username.ToLower();

            var user = await _userRepository
                .Get(x => x.Email.Equals(username))
                .Select(x => new UserResult
                {
                    DisplayName = x.DisplayName,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    UserName = x.UserName,
                    UpdatedDate = x.UpdatedDate,
                    CreatedDate = x.CreatedDate,
                    UpdatedById = x.UpdatedById,
                    CreatedById = x.CreatedById,
                    StatusId = x.StatusId,
                    Email = x.Email,
                    PasswordHash = x.PasswordHash,
                    SecurityStamp = x.SecurityStamp,
                    Id = x.Id,
                    IsEmailConfirmed = x.IsEmailConfirmed,
                    GenderId = x.UserInfo.GenderId,
                    Address = x.UserInfo.Address,
                    BirthDate = x.UserInfo.BirthDate,
                    CountryId = x.UserInfo.CountryId,
                    PhoneNumber = x.UserInfo.PhoneNumber,
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserResult> FindByIdAsync(long id)
        {
            var existUser = await _userRepository
                .Get(x => x.Id.Equals(id))
                .Select(x => new UserResult()
                {
                    DisplayName = x.DisplayName,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    UserName = x.UserName,
                    UpdatedDate = x.UpdatedDate,
                    CreatedDate = x.CreatedDate,
                    UpdatedById = x.UpdatedById,
                    CreatedById = x.CreatedById,
                    StatusId = x.StatusId,
                    Email = x.Email,
                    PasswordHash = x.PasswordHash,
                    SecurityStamp = x.SecurityStamp,
                    Id = x.Id,
                    IsEmailConfirmed = x.IsEmailConfirmed
                })
                .FirstOrDefaultAsync();

            return existUser;
        }

        public async Task<IList<UserResult>> GetNameByIdsAsync(IEnumerable<long> ids)
        {
            var existUsers = await _userRepository
                .Get(x => x.Id.In(ids))
                .Select(x => new UserResult()
                {
                    DisplayName = x.DisplayName,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    UserName = x.UserName,
                    Id = x.Id,
                    IsEmailConfirmed = x.IsEmailConfirmed
                })
                .ToListAsync();

            return existUsers;
        }

        public async Task<UserFullResult> FindFullByIdAsync(long id)
        {
            var existUser = await _userRepository
                .Get(x => x.Id.Equals(id))
                .Select(x => new UserFullResult()
                {
                    CreatedDate = x.CreatedDate,
                    DisplayName = x.DisplayName,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    UserName = x.UserName,
                    Email = x.Email,
                    PhoneNumber = x.UserInfo.PhoneNumber,
                    Description = x.UserInfo.Description,
                    Address = x.UserInfo.Address,
                    BirthDate = x.UserInfo.BirthDate,
                    GenderId = x.UserInfo.GenderId,
                    GenderLabel = x.UserInfo.Gender.Name,
                    StatusId = x.StatusId,
                    StatusLabel = x.Status.Name,
                    Id = x.Id,
                    CountryId = x.UserInfo.CountryId,
                    CountryCode = x.UserInfo.Country.Code,
                    CountryName = x.UserInfo.Country.Name
                })
                .FirstOrDefaultAsync();

            return existUser;
        }

        public List<UserFullResult> Search(string query = "", List<long> currentUserIds = null, int page = 1, int pageSize = 10)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            query = query.ToLower();

            var hasCurrentUserIds = currentUserIds != null && currentUserIds.Any();
            var data = _userRepository.Get(x => string.IsNullOrEmpty(query) || x.Lastname.ToLower().Contains(query)
                || x.Firstname.ToLower().Contains(query) || x.DisplayName.ToLower().Contains(query))
                .Where(x => !hasCurrentUserIds || x.Id.NotIn(currentUserIds));

            if (pageSize > 0)
            {
                data = data.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var users = data
                .Select(x => new UserFullResult()
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

        public async Task<BasePageList<UserFullResult>> GetAsync(UserFilter filter)
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

            if (filter.ExclusiveCreatedById.HasValue)
            {
                userQuery = userQuery.Where(x => x.Id != filter.ExclusiveCreatedById);
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
                         select new UserFullResult()
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

            var result = new BasePageList<UserFullResult>(users)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }
        #endregion
    }
}
