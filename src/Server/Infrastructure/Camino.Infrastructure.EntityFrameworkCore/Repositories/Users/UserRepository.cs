using Camino.Core.Contracts.Data;
using Camino.Shared.Results.Identifiers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Enums;
using System.Collections.Generic;
using Camino.Shared.Requests.Filters;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Shared.Requests.UpdateItems;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Requests.Identifiers;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions;
using Camino.Core.Contracts.Validations;
using Camino.Core.Validations;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Users
{
    public partial class UserRepository : IUserRepository, IScopedDependency
    {
        #region Fields/Properties
        private readonly IEntityRepository<UserInfo> _userInfoRepository;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IValidationStrategyContext _validationStrategyContext;
        private readonly IAppDbContext _dbContext;
        private int _userDeletedStatus;
        private int _userInactivedStatus;
        #endregion

        #region Ctor
        public UserRepository(IEntityRepository<User> userRepository, IValidationStrategyContext validationStrategyContext,
            IEntityRepository<UserInfo> userInfoRepository, IAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
            _userDeletedStatus = UserStatus.Deleted.GetCode();
            _userInactivedStatus = UserStatus.Inactived.GetCode();
        }
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(UserModifyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var newUser = new User
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
                        StatusId = UserStatus.Pending.GetCode(),
                        UserName = request.UserName,
                        DisplayName = request.DisplayName,
                        IsEmailConfirmed = true
                    };
                    await _userRepository.InsertAsync(newUser);

                    if (newUser.Id > 0)
                    {
                        await _userInfoRepository.InsertAsync(new UserInfo
                        {
                            BirthDate = request.BirthDate,
                            Description = request.Description,
                            GenderId = request.GenderId,
                            Id = newUser.Id
                        });
                        transaction.Commit();

                        return newUser.Id;
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

        public async Task<bool> DeleteAsync(long id)
        {
            await _userRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(UserModifyRequest request)
        {
            var existing = await _userRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = UserStatus.Deleted.GetCode();
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeactivateAsync(UserModifyRequest request)
        {
            var existing = await _userRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = UserStatus.Inactived.GetCode();
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ActiveAsync(UserModifyRequest request)
        {
            var existing = await _userRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = UserStatus.Actived.GetCode();
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ConfirmAsync(UserModifyRequest request)
        {
            var existing = await _userRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = UserStatus.Actived.GetCode();
            existing.IsEmailConfirmed = true;
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<PartialUpdateRequest> PartialUpdateAsync(PartialUpdateRequest request)
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
            var userInfo = _userInfoRepository.Find(x => x.Id == key);
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

            await _dbContext.UpdateByNameAsync(userInfo, request.Value, request.PropertyName, true);

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

            var existing = await _userRepository.FindAsync(x => x.Id == request.Id);
            existing.UpdatedById = request.Id;
            existing.UpdatedDate = DateTimeOffset.UtcNow;
            existing.Lastname = request.Lastname;
            existing.Firstname = request.Firstname;
            existing.DisplayName = request.DisplayName;

            await _dbContext.SaveChangesAsync();
            return request;
        }

        public async Task<bool> UpdateAsync(UserModifyRequest request)
        {
            var existing = await _userRepository.FindAsync(x => x.Id == request.Id);
            existing.UpdatedById = request.Id;
            existing.UpdatedDate = DateTime.UtcNow;
            existing.Lastname = request.Lastname;
            existing.Firstname = request.Firstname;
            existing.DisplayName = request.DisplayName;
            existing.IsEmailConfirmed = request.IsEmailConfirmed;
            existing.PasswordHash = request.PasswordHash;

            return (await _dbContext.SaveChangesAsync()) > 0;
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
                .Get(x => ids.Contains(x.Id))
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

        public async Task<UserFullResult> FindFullByIdAsync(IdRequestFilter<long> filter)
        {
            var existUser = await _userRepository
                .Get(x => x.Id.Equals(filter.Id) && (x.StatusId == _userDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _userInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _userDeletedStatus && x.StatusId != _userInactivedStatus))
                .Select(x => new UserFullResult
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
                    UpdatedDate = x.UpdatedDate,
                    CountryId = x.UserInfo.CountryId,
                    CountryCode = x.UserInfo.Country.Code,
                    CountryName = x.UserInfo.Country.Name,
                    IsEmailConfirmed = x.IsEmailConfirmed
                })
                .FirstOrDefaultAsync();

            return existUser;
        }

        public async Task<List<UserFullResult>> SearchAsync(UserFilter filter, List<long> currentUserIds = null)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var userQuery = _userRepository.Get(x => (x.StatusId == _userDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _userInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _userDeletedStatus && x.StatusId != _userInactivedStatus));

            if (!string.IsNullOrEmpty(search))
            {
                userQuery = userQuery.Where(user => user.Lastname.ToLower().Contains(search)
                         || user.Firstname.ToLower().Contains(search)
                         || (user.Lastname + " " + user.Firstname).ToLower().Contains(search)
                         || user.Email.Contains(search)
                         || user.DisplayName.ToLower().Contains(search));
            }

            if (currentUserIds != null && currentUserIds.Any())
            {
                userQuery = userQuery.Where(x => !currentUserIds.Contains(x.Id));
            }

            if (filter.PageSize > 0)
            {
                userQuery = userQuery.Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            var users = await userQuery
                .Select(x => new UserFullResult()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Lastname = x.Lastname,
                    Firstname = x.Firstname,
                    DisplayName = x.DisplayName
                })
                .ToListAsync();

            return users;
        }

        public async Task<BasePageList<UserFullResult>> GetAsync(UserFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var userQuery = _userRepository.Get(x => (x.StatusId == _userDeletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == _userInactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != _userDeletedStatus && x.StatusId != _userInactivedStatus));

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

            if (filter.ExclusiveUserById.HasValue)
            {
                userQuery = userQuery.Where(x => x.Id != filter.ExclusiveUserById);
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
                             StatusId = user.StatusId,
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
