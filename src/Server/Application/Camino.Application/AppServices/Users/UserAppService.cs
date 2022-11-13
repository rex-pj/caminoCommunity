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
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.AppServices.Users
{
    public class UserAppService : IUserAppService, IScopedDependency
    {
        #region Fields/Properties
        private readonly BaseValidatorContext _validatorContext;
        private readonly IUserRepository _userRepository;
        private readonly IEntityRepository<User> _userEntityRepository;
        private readonly IUserDomainService _userDomainService;
        private readonly int _userDeletedStatus;
        private readonly int _userInactivedStatus;
        #endregion

        #region Ctor
        public UserAppService(IUserRepository userRepository,
            BaseValidatorContext validatorContext,
            IEntityRepository<User> userEntityRepository,
            IUserDomainService userDomainService)
        {
            _userRepository = userRepository;
            _userEntityRepository = userEntityRepository;
            _userDomainService = userDomainService;
            _validatorContext = validatorContext;
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

            var modifiedDate = DateTime.UtcNow;
            var newUser = new User
            {
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Email = request.Email,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                PasswordHash = request.PasswordHash,
                SecurityStamp = request.SecurityStamp,
                StatusId = UserStatuses.Pending.GetCode(),
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                IsEmailConfirmed = true,
                BirthDate = request.BirthDate,
                Description = request.Description,
                GenderId = request.GenderId
            };

            var id = await _userRepository.CreateAsync(newUser);
            newUser.CreatedById = id;
            newUser.UpdatedById = id;
            await _userRepository.UpdateAsync(newUser);
            return id;
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
            var user = await _userEntityRepository.FindAsync(x => x.Id == (long)request.Key);
            if (user == null)
            {
                throw new ArgumentException(nameof(user));
            }

            foreach (var updateItem in request.Updates)
            {
                PartialUpdateItem(user, updateItem);
            }

            user.UpdatedById = user.Id;
            user.UpdatedDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return request;
        }

        private void PartialUpdateItem(User user, PartialUpdateItemRequest updateItem)
        {
            if (nameof(User.Address).EqualsIgnoreCase(updateItem.PropertyName))
            {
                user.Address = updateItem.Value?.ToString();
            }
            else if (nameof(User.PhoneNumber).EqualsIgnoreCase(updateItem.PropertyName))
            {
                _validatorContext.SetValidator(new PhoneValidator());
                bool isValid = updateItem.Value == null || string.IsNullOrEmpty(updateItem.Value.ToString()) || _validatorContext.Validate<object, bool>(updateItem.Value);
                if (isValid)
                {
                    user.PhoneNumber = updateItem.Value.ToString();
                }
            }
            else if (nameof(User.Description).EqualsIgnoreCase(updateItem.PropertyName))
            {
                user.Description = updateItem.Value?.ToString();
            }
            else if (nameof(User.BirthDate).EqualsIgnoreCase(updateItem.PropertyName))
            {
                if (DateTime.TryParse(updateItem.Value.ToString(), out var birthDate))
                {
                    user.BirthDate = birthDate;
                }
                else if (updateItem.Value == null)
                {
                    user.BirthDate = null;
                }
            }
            else if (nameof(User.GenderId).EqualsIgnoreCase(updateItem.PropertyName))
            {
                if (int.TryParse(updateItem.Value.ToString(), out var genderId))
                {
                    user.GenderId = genderId;
                }
                else if (updateItem.Value == null)
                {
                    user.GenderId = null;
                }
            }
            else if (nameof(User.CountryId).EqualsIgnoreCase(updateItem.PropertyName))
            {
                if (short.TryParse(updateItem.Value.ToString(), out var countryId))
                {
                    user.CountryId = countryId;
                }
                else if (updateItem.Value == null)
                {
                    user.CountryId = null;
                }
            }
            else
            {
                throw new NotSupportedException($"Not support {updateItem.PropertyName}");
            }
        }

        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(UserIdentifierUpdateRequest request)
        {
            _validatorContext.SetValidator(new UserProfileValidator());
            bool canUpdate = _validatorContext.Validate<UserIdentifierUpdateRequest, bool>(request);
            if (!canUpdate)
            {
                foreach (var item in _validatorContext.Errors)
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

            if (filter.GenderId.HasValue)
            {
                userQuery = userQuery.Where(x => x.GenderId == filter.GenderId);
            }

            if (filter.CountryId.HasValue)
            {
                userQuery = userQuery.Where(x => x.CountryId == filter.CountryId);
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                userQuery = userQuery.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                userQuery = userQuery.Where(x => x.Address.Contains(filter.Address));
            }

            // Filter by birthdate
            if (filter.BirthDateFrom.HasValue && filter.BirthDateTo.HasValue)
            {
                userQuery = userQuery.Where(x => x.BirthDate >= filter.BirthDateFrom && x.BirthDate <= filter.BirthDateTo);
            }
            else if (filter.BirthDateTo.HasValue)
            {
                userQuery = userQuery.Where(x => x.BirthDate <= filter.BirthDateTo);
            }
            else if (filter.BirthDateFrom.HasValue)
            {
                userQuery = userQuery.Where(x => x.BirthDate >= filter.BirthDateFrom && x.BirthDate <= DateTime.UtcNow);
            }

            var query = (from user in userQuery
                         join userInfo in userQuery
                         on user.Id equals userInfo.Id
                         select new UserFullResult()
                         {
                             Id = user.Id,
                             Email = user.Email,
                             Address = user.Address,
                             Lastname = user.Lastname,
                             Firstname = user.Firstname,
                             DisplayName = user.DisplayName,
                             CreatedDate = user.CreatedDate,
                             UpdatedDate = user.UpdatedDate,
                             BirthDate = user.BirthDate,
                             IsEmailConfirmed = user.IsEmailConfirmed,
                             PhoneNumber = user.PhoneNumber,
                             GenderLabel = user.Gender.Name,
                             StatusLabel = user.Status.Name,
                             StatusId = user.StatusId,
                             CountryName = user.Country.Name
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
            if (entity == null)
            {
                return null;
            }

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
                GenderId = entity.GenderId,
                Address = entity.Address,
                BirthDate = entity.BirthDate,
                CountryId = entity.CountryId,
                PhoneNumber = entity.PhoneNumber,
            };
        }

        private UserFullResult MapEntityToFullDto(User entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new UserFullResult
            {
                CreatedDate = entity.CreatedDate,
                DisplayName = entity.DisplayName,
                Firstname = entity.Firstname,
                Lastname = entity.Lastname,
                UserName = entity.UserName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Description = entity.Description,
                Address = entity.Address,
                BirthDate = entity.BirthDate,
                GenderId = entity.GenderId,
                GenderLabel = entity.Gender?.Name,
                StatusId = entity.StatusId,
                StatusLabel = entity.Status.Name,
                Id = entity.Id,
                UpdatedDate = entity.UpdatedDate,
                CountryId = entity.CountryId,
                CountryCode = entity.Country?.Code,
                CountryName = entity.Country?.Name,
                IsEmailConfirmed = entity.IsEmailConfirmed
            };
        }
        #endregion
    }
}
