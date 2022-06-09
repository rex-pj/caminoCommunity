using Camino.Core.Contracts.Repositories.Users;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Shared.Enums;
using Camino.Core.Domains.Users;
using Camino.Core.Domains;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Shared.Utils;
using Camino.Application.Contracts;
using Camino.Core.Validators;
using Camino.Application.Validators;
using Camino.Core.Domains.Users.DomainServices;
using Camino.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Application.AppServices.Users
{
    public class UserAppService : IUserAppService, IScopedDependency
    {
        #region Fields/Properties
        private readonly IValidationStrategyContext _validationStrategyContext;
        private readonly IUserRepository _userRepository;
        private readonly IEntityRepository<User> _userEntityRepository;
        private readonly IEntityRepository<UserInfo> _userInfoEntityRepository;
        private readonly IUserDomainService _userDomainService;
        private readonly int _userDeletedStatus;
        private readonly int _userInactivedStatus;
        #endregion

        #region Ctor
        public UserAppService(IUserRepository userRepository,
            IValidationStrategyContext validationStrategyContext,
            IEntityRepository<User> userEntityRepository,
            IUserDomainService userDomainService,
            IEntityRepository<UserInfo> userInfoEntityRepository)
        {
            _userRepository = userRepository;
            _userEntityRepository = userEntityRepository;
            _userInfoEntityRepository = userInfoEntityRepository;
            _userDomainService = userDomainService;
            _validationStrategyContext = validationStrategyContext;
            _userDeletedStatus = UserStatuses.Deleted.GetCode();
            _userInactivedStatus = UserStatuses.Inactived.GetCode();
        }
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(UserModifyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

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
                StatusId = UserStatuses.Pending.GetCode(),
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                IsEmailConfirmed = true,
                UserInfo = new UserInfo
                {
                    BirthDate = request.BirthDate,
                    Description = request.Description,
                    GenderId = request.GenderId
                }
            };

            return await _userRepository.CreateAsync(newUser);
        }

        public async Task<bool> SoftDeleteAsync(long id, long updatedById)
        {
            return await _userRepository.UpdateStatusAsync(id, updatedById, UserStatuses.Deleted);
        }

        public async Task<bool> ActiveAsync(long id, long updatedById)
        {
            return await _userRepository.UpdateStatusAsync(id, updatedById, UserStatuses.Actived);
        }

        public async Task<bool> DeactivateAsync(long id, long updatedById)
        {
            return await _userRepository.UpdateStatusAsync(id, updatedById, UserStatuses.Inactived);
        }

        public async Task<bool> ConfirmAsync(long id, long updatedById)
        {
            return await _userDomainService.ConfirmAsync(id, updatedById);
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
            var userInfo = _userInfoEntityRepository.Find(x => x.Id == key);
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

            await _userRepository.PartialUpdateAsync(userInfo, request.PropertyName, request.Value);
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

            var existing = await _userRepository.FindByIdAsync(request.Id);
            existing.UpdatedById = request.Id;
            existing.Lastname = request.Lastname;
            existing.Firstname = request.Firstname;
            existing.DisplayName = request.DisplayName;

            await _userRepository.UpdateAsync(existing);
            return request;
        }

        public async Task<bool> UpdateAsync(UserModifyRequest request)
        {
            var existing = await _userRepository.FindByIdAsync(request.Id);
            existing.UpdatedById = request.Id;
            existing.Lastname = request.Lastname;
            existing.Firstname = request.Firstname;
            existing.DisplayName = request.DisplayName;
            existing.IsEmailConfirmed = request.IsEmailConfirmed;
            existing.PasswordHash = request.PasswordHash;
            return await _userRepository.UpdateAsync(existing);
        }
        #endregion

        #region GET
        public async Task<UserResult> FindByEmailAsync(string email)
        {
            var existing = await _userRepository.FindByEmailAsync(email);
            return MapEntityToDto(existing);
        }

        public async Task<UserResult> FindByUsernameAsync(string username)
        {
            var existing = await _userRepository.FindByUsernameAsync(username);
            return MapEntityToDto(existing);
        }

        public async Task<UserResult> FindByIdAsync(long id)
        {
            var existing = await _userRepository.FindByIdAsync(id);
            return MapEntityToDto(existing);
        }

        public async Task<UserFullResult> FindFullByIdAsync(IdRequestFilter<long> filter)
        {
            var existing = await _userRepository.FindByIdAsync(filter.Id);
            return MapEntityToFullDto(existing);
        }

        public async Task<List<UserFullResult>> SearchAsync(UserFilter filter, List<long> currentUserIds = null)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var userQuery = _userEntityRepository.Get(x => (x.StatusId == _userDeletedStatus && filter.CanGetDeleted)
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
            var userQuery = _userEntityRepository.Get(x => (x.StatusId == _userDeletedStatus && filter.CanGetDeleted)
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
            var userInfoQuery = _userInfoEntityRepository.Table;
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

        private UserResult MapEntityToDto(User entity)
        {
            return new UserResult
            {
                DisplayName = entity.DisplayName,
                Firstname = entity.Firstname,
                Lastname = entity.Lastname,
                UserName = entity.UserName,
                UpdatedDate = entity.UpdatedDate,
                CreatedDate = entity.CreatedDate,
                UpdatedById = entity.UpdatedById,
                CreatedById = entity.CreatedById,
                StatusId = entity.StatusId,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                SecurityStamp = entity.SecurityStamp,
                Id = entity.Id,
                IsEmailConfirmed = entity.IsEmailConfirmed,
                GenderId = entity.UserInfo.GenderId,
                Address = entity.UserInfo.Address,
                BirthDate = entity.UserInfo.BirthDate,
                CountryId = entity.UserInfo.CountryId,
                PhoneNumber = entity.UserInfo.PhoneNumber,
            };
        }

        private UserFullResult MapEntityToFullDto(User entity)
        {
            return new UserFullResult
            {
                CreatedDate = entity.CreatedDate,
                DisplayName = entity.DisplayName,
                Firstname = entity.Firstname,
                Lastname = entity.Lastname,
                UserName = entity.UserName,
                Email = entity.Email,
                PhoneNumber = entity.UserInfo.PhoneNumber,
                Description = entity.UserInfo.Description,
                Address = entity.UserInfo.Address,
                BirthDate = entity.UserInfo.BirthDate,
                GenderId = entity.UserInfo.GenderId,
                GenderLabel = entity.UserInfo.Gender.Name,
                StatusId = entity.StatusId,
                StatusLabel = entity.Status.Name,
                Id = entity.Id,
                UpdatedDate = entity.UpdatedDate,
                CountryId = entity.UserInfo.CountryId,
                CountryCode = entity.UserInfo.Country.Code,
                CountryName = entity.UserInfo.Country.Name,
                IsEmailConfirmed = entity.IsEmailConfirmed
            };
        }
        #endregion
    }
}
