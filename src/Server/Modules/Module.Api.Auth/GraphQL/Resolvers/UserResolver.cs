using Camino.Framework.Models;
using Camino.Framework.GraphQL.Resolvers;
using Camino.Shared.Enums;
using System;
using System.Threading.Tasks;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Linq;
using Microsoft.Extensions.Options;
using Camino.Core.Domain.Identities;
using Camino.Shared.Configurations;
using Camino.Core.Contracts.Services.Users;
using Camino.Shared.Requests.Filters;
using System.Collections.Generic;
using Camino.Shared.Results.Media;
using Camino.Core.Contracts.IdentityManager;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.General;
using Camino.Shared.Requests.UpdateItems;
using Camino.Shared.Results.Identifiers;
using Camino.Shared.Requests.Providers;
using Camino.Core.Contracts.Providers;
using Camino.Infrastructure.Resources;
using System.Security.Claims;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IUserPhotoService _userPhotoService;
        private readonly IEmailProvider _emailSender;
        private readonly AppSettings _appSettings;
        private readonly RegisterConfirmationSettings _registerConfirmationSettings;
        private readonly PagerOptions _pagerOptions;

        public UserResolver(IUserManager<ApplicationUser> userManager, IEmailProvider emailSender,
            IUserService userService, IOptions<AppSettings> appSettings,
            IUserPhotoService userPhotoService, IOptions<RegisterConfirmationSettings> registerConfirmationSettings, IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _userManager = userManager;
            _userService = userService;
            _appSettings = appSettings.Value;
            _registerConfirmationSettings = registerConfirmationSettings.Value;
            _emailSender = emailSender;
            _userPhotoService = userPhotoService;
            _pagerOptions = pagerOptions.Value;
        }

        #region Get
        public async Task<UserInfoModel> GetLoggedUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            long currentUserId = GetCurrentUserId(claimsPrincipal);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var userIdentityId = await _userManager.EncryptUserIdAsync(currentUserId);
            return new UserInfoModel
            {
                Address = currentUser.Address,
                BirthDate = currentUser.BirthDate,
                CountryCode = currentUser.CountryCode,
                CountryId = currentUser.CountryId,
                CountryName = currentUser.CountryName,
                Email = currentUser.Email,
                CreatedDate = currentUser.CreatedDate,
                Description = currentUser.Description,
                DisplayName = currentUser.DisplayName,
                Firstname = currentUser.Firstname,
                GenderId = currentUser.GenderId,
                GenderLabel = currentUser.GenderLabel,
                Lastname = currentUser.Lastname,
                PhoneNumber = currentUser.PhoneNumber,
                StatusId = currentUser.StatusId,
                StatusLabel = currentUser.StatusLabel,
                UpdatedDate = currentUser.UpdatedDate,
                UserIdentityId = await _userManager.EncryptUserIdAsync(currentUserId)
            };
        }

        public async Task<UserPageListModel> GetUsersAsync(UserFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new UserFilterModel();
            }

            var filterRequest = new UserFilter
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize.HasValue && criterias.PageSize < _pagerOptions.PageSize ? criterias.PageSize.Value : _pagerOptions.PageSize,
                Keyword = criterias.Search,
            };

            if (!string.IsNullOrEmpty(criterias.ExclusiveUserIdentityId))
            {
                filterRequest.ExclusiveUserById = await _userManager.DecryptUserIdAsync(criterias.ExclusiveUserIdentityId);
            }

            try
            {
                var userPageList = await _userService.GetAsync(filterRequest);
                var userIds = userPageList.Collections.Select(x => x.Id);
                var userPhotos = await _userPhotoService.GetUserPhotoByUserIdsAsync(userIds, UserPictureType.Avatar);
                var users = await MapUsersResultToModelAsync(userPageList.Collections, userPhotos);

                var userPage = new UserPageListModel(users)
                {
                    Filter = criterias,
                    TotalPage = userPageList.TotalPage,
                    TotalResult = userPageList.TotalResult
                };

                return userPage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SelectOption>> SelectUsersAsync(UserFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new UserFilterModel();
            }

            var exclusiveUserIds = new List<long>();
            if (!string.IsNullOrEmpty(criterias.ExclusiveUserIdentityId))
            {
                exclusiveUserIds.Add(await _userManager.DecryptUserIdAsync(criterias.ExclusiveUserIdentityId));
            }

            var data = await _userService.SearchAsync(new UserFilter
            {
                Page = criterias.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = criterias.Search,
            }, exclusiveUserIds);
            var users = await MapUsersResultToModelAsync(data);

            var userSelections = users.Select(x => new SelectOption
            {
                Id = x.UserIdentityId,
                Text = x.DisplayName
            });

            return userSelections;
        }

        public async Task<UserInfoModel> GetFullUserInfoAsync(ClaimsPrincipal claimsPrincipal, FindUserModel criterias)
        {
            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var userId = currentUserId;
            if (!string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            var user = await _userService.FindFullByIdAsync(new IdRequestFilter<long>
            {
                Id = userId,
                CanGetInactived = currentUserId == userId
            });

            if (user == null)
            {
                return null;
            }

            var userInfo = new UserInfoModel
            {
                Address = user.Address,
                BirthDate = user.BirthDate,
                CountryCode = user.CountryCode,
                CountryId = user.CountryId,
                CountryName = user.CountryName,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                Description = user.Description,
                DisplayName = user.DisplayName,
                Firstname = user.Firstname,
                GenderId = user.GenderId,
                GenderLabel = user.GenderLabel,
                Lastname = user.Lastname,
                PhoneNumber = user.PhoneNumber,
                StatusId = user.StatusId,
                StatusLabel = user.StatusLabel,
                UpdatedDate = user.UpdatedDate,
                CanEdit = userId == currentUserId,
            };

            userInfo.UserIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
            return userInfo;
        }

        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(ClaimsPrincipal claimsPrincipal, UserIdentifierUpdateModel criterias)
        {
            try
            {
                var currentUserId = GetCurrentUserId(claimsPrincipal);
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                currentUser.Lastname = criterias.Lastname;
                currentUser.Firstname = criterias.Firstname;
                currentUser.DisplayName = criterias.DisplayName;

                var updatedUser = await _userManager.UpdateAsync(currentUser);
                if (updatedUser.Succeeded)
                {
                    return new UserIdentifierUpdateRequest()
                    {
                        DisplayName = currentUser.DisplayName,
                        Firstname = currentUser.Firstname,
                        Id = currentUser.Id,
                        Lastname = currentUser.Lastname
                    };
                }
                return new UserIdentifierUpdateRequest();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTokenModel> UpdatePasswordAsync(ClaimsPrincipal claimsPrincipal, UserPasswordUpdateModel criterias)
        {
            try
            {
                ComparePassword(criterias);
                var currentUserId = GetCurrentUserId(claimsPrincipal);
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                var result = await _userManager.ChangePasswordAsync(currentUser, criterias.CurrentPassword, criterias.NewPassword);
                if (!result.Succeeded)
                {
                    return new UserTokenModel(false);
                }

                return new UserTokenModel()
                {
                    AuthenticationToken = currentUser.AuthenticationToken,
                    AccessMode = AccessMode.CanEdit,
                    IsSucceed = true,
                    UserInfo = new UserInfoModel()
                    {
                        Address = currentUser.Address,
                        BirthDate = currentUser.BirthDate,
                        CountryCode = currentUser.CountryCode,
                        CountryId = currentUser.CountryId,
                        CountryName = currentUser.CountryName,
                        Email = currentUser.Email,
                        CreatedDate = currentUser.CreatedDate,
                        Description = currentUser.Description,
                        DisplayName = currentUser.DisplayName,
                        Firstname = currentUser.Firstname,
                        GenderId = currentUser.GenderId,
                        GenderLabel = currentUser.GenderLabel,
                        Lastname = currentUser.Lastname,
                        PhoneNumber = currentUser.PhoneNumber,
                        StatusId = currentUser.StatusId,
                        StatusLabel = currentUser.StatusLabel,
                        UpdatedDate = currentUser.UpdatedDate,
                        UserIdentityId = currentUser.UserIdentityId
                    }
                };
            }
            catch (Exception)
            {
                return new UserTokenModel(false);
            }
        }
        #endregion

        #region CRUD
        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(ClaimsPrincipal claimsPrincipal, UpdatePerItemModel criterias)
        {
            try
            {
                ValidateUserInfoItem(criterias);
                var currentUserId = GetCurrentUserId(claimsPrincipal);
                var userId = await _userManager.DecryptUserIdAsync(criterias.Key.ToString());
                if (userId != currentUserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var updatePerItem = new UpdateItemRequest()
                {
                    Key = criterias.Key,
                    PropertyName = criterias.PropertyName,
                    Value = criterias.Value
                };
                updatePerItem.Key = userId;
                var updatedItem = await _userService.UpdateInfoItemAsync(updatePerItem);
                return new UpdatePerItemModel()
                {
                    Key = updatedItem.Key.ToString(),
                    PropertyName = updatedItem.PropertyName,
                    Value = updatedItem.Value.ToString()
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void ValidateUserInfoItem(UpdatePerItemModel criterias)
        {
            if (!criterias.CanEdit)
            {
                throw new UnauthorizedAccessException();
            }

            if (criterias.PropertyName == null)
            {
                throw new ArgumentException(nameof(criterias.PropertyName));
            }

            if (criterias.Key == null || string.IsNullOrEmpty(criterias.Key.ToString()))
            {
                throw new ArgumentException(nameof(criterias.Key));
            }
        }

        private void ComparePassword(UserPasswordUpdateModel criterias)
        {
            if (!criterias.NewPassword.Equals(criterias.ConfirmPassword))
            {
                throw new ArgumentException($"{nameof(criterias.NewPassword)} and {nameof(criterias.ConfirmPassword)} is not the same");
            }
        }

        private async Task<IList<UserInfoModel>> MapUsersResultToModelAsync(IEnumerable<UserFullResult> userResults, IList<UserPhotoResult> userPhotos = null)
        {
            var users = new List<UserInfoModel>();
            foreach (var userResult in userResults)
            {
                var user = new UserInfoModel()
                {
                    Address = userResult.Address,
                    BirthDate = userResult.BirthDate,
                    CountryCode = userResult.CountryCode,
                    CountryId = userResult.CountryId,
                    CountryName = userResult.CountryName,
                    Email = userResult.Email,
                    CreatedDate = userResult.CreatedDate,
                    Description = userResult.Description,
                    DisplayName = userResult.DisplayName,
                    Firstname = userResult.Firstname,
                    GenderId = userResult.GenderId,
                    GenderLabel = userResult.GenderLabel,
                    Lastname = userResult.Lastname,
                    PhoneNumber = userResult.PhoneNumber,
                    StatusId = userResult.StatusId,
                    StatusLabel = userResult.StatusLabel,
                    UpdatedDate = userResult.UpdatedDate,
                    UserIdentityId = await _userManager.EncryptUserIdAsync(userResult.Id)
                };

                if (userPhotos != null && userPhotos.Any())
                {
                    var userAvatar = userPhotos.FirstOrDefault(x => x.UserId == userResult.Id);
                    user.AvatarCode = userAvatar.Code;
                }

                users.Add(user);
            }

            return users;
        }

        public async Task<CommonResult> SignupAsync(SignupModel criterias)
        {
            var user = new ApplicationUser()
            {
                BirthDate = criterias.BirthDate,
                DisplayName = $"{criterias.Lastname} {criterias.Firstname}",
                Email = criterias.Email,
                Firstname = criterias.Firstname,
                Lastname = criterias.Lastname,
                GenderId = (byte)criterias.GenderId,
                StatusId = (byte)UserStatus.Pending,
                UserName = criterias.Email,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, criterias.Password);
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(user.UserName);
                    var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await SendActiveEmailAsync(user, confirmationToken);
                }
                else
                {
                    return CommonResult.Failed(result.Errors.Select(x => new CommonError()
                    {
                        Code = x.Code,
                        Message = x.Description
                    }));
                }

                return CommonResult.Success();
            }
            catch (Exception ex)
            {
                return CommonResult.Failed(new CommonError()
                {
                    Message = ex.Message
                });
            }
        }

        private async Task SendActiveEmailAsync(ApplicationUser user, string confirmationToken)
        {
            var activeUserUrl = $"{_registerConfirmationSettings.Url}/{user.Email}/{confirmationToken}";
            await _emailSender.SendEmailAsync(new MailMessageRequest()
            {
                Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appSettings.ApplicationName, activeUserUrl),
                FromEmail = _registerConfirmationSettings.FromEmail,
                FromName = _registerConfirmationSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _appSettings.ApplicationName),
            }, EmailTextFormat.Html);
        }

        public async Task<CommonResult> ActiveAsync(ActiveUserModel criterias)
        {
            var user = await _userManager.FindByNameAsync(criterias.Email);
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                var result = await _userManager.ConfirmEmailAsync(user, criterias.ActiveKey);
                if (result.Succeeded)
                {
                    return CommonResult.Success();
                }

                var errors = result.Errors.Select(x => new CommonError()
                {
                    Message = x.Description,
                    Code = x.Code
                });

                return CommonResult.Failed(errors);
            }

            return CommonResult.Failed(new CommonError()
            {
                Message = "The user is already confirmed"
            });
        }
    }
}
