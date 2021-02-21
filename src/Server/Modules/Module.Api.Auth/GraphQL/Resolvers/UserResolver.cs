using Camino.Framework.Models;
using Camino.Framework.GraphQL.Resolvers;
using Camino.Shared.Enums;
using System;
using System.Threading.Tasks;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using Camino.Core.Constants;
using Camino.Core.Resources;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Camino.Core.Exceptions;
using Microsoft.Extensions.Options;
using Camino.Core.Domain.Identities;
using Camino.IdentityManager.Contracts.Core;
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

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IUserService _userService;
        private readonly IUserPhotoService _userPhotoService;
        private readonly IEmailProvider _emailSender;
        private readonly AppSettings _appSettings;
        private readonly RegisterConfirmationSettings _registerConfirmationSettings;
        private readonly ResetPasswordSettings _resetPasswordSettings;

        public UserResolver(IUserManager<ApplicationUser> userManager, ILoginManager<ApplicationUser> loginManager, IEmailProvider emailSender,
            IUserService userService, IOptions<AppSettings> appSettings, ISessionContext sessionContext, IOptions<ResetPasswordSettings> resetPasswordSettings,
            IUserPhotoService userPhotoService, IOptions<RegisterConfirmationSettings> registerConfirmationSettings)
            : base(sessionContext)
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _userService = userService;
            _appSettings = appSettings.Value;
            _registerConfirmationSettings = registerConfirmationSettings.Value;
            _resetPasswordSettings = resetPasswordSettings.Value;
            _emailSender = emailSender;
            _userPhotoService = userPhotoService;
        }

        public UserInfoModel GetLoggedUser(ApplicationUser currentUser)
        {
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
                UserIdentityId = currentUser.UserIdentityId
            };
        }

        public async Task<UserPageListModel> GetUsersAsync(UserFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new UserFilterModel();
            }

            var filterRequest = new UserFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search
            };

            if (!string.IsNullOrEmpty(criterias.ExclusiveCreatedIdentityId))
            {
                filterRequest.ExclusiveCreatedById = await _userManager.DecryptUserIdAsync(criterias.ExclusiveCreatedIdentityId);
            }

            try
            {
                var userPageList = await _userService.GetAsync(filterRequest);

                var userIds = userPageList.Collections.Select(x => x.Id);
                var userPhotos = _userPhotoService.GetUserPhotoByUserIds(userIds, UserPhotoKind.Avatar);
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

        public async Task<UserInfoModel> GetFullUserInfoAsync(ApplicationUser currentUser, FindUserModel criterias)
        {
            var userId = currentUser.Id;
            if (!string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            var user = await _userService.FindFullByIdAsync(userId);
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
                CanEdit = userId == currentUser.Id
            };

            userInfo.UserIdentityId = await _userManager.EncryptUserIdAsync(user.Id);

            return userInfo;
        }

        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(ApplicationUser currentUser, UpdatePerItemModel criterias)
        {
            try
            {
                ValidateUserInfoItem(criterias);

                var userId = await _userManager.DecryptUserIdAsync(criterias.Key.ToString());
                if (userId != currentUser.Id)
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

        public async Task<CommonResult> LogoutAsync(ApplicationUser currentUser)
        {
            try
            {
                var result = await _userManager.RemoveLoginAsync(currentUser, ServiceProvidersNameConst.CAMINO_API_AUTH, currentUser.AuthenticationToken);
                if (!result.Succeeded)
                {
                    return CommonResult.Failed(new CommonError()
                    {
                        Code = ErrorMessageConst.EXCEPTION,
                        Message = ErrorMessageConst.UN_EXPECTED_EXCEPTION
                    });
                }
                return CommonResult.Success();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(ApplicationUser currentUser, UserIdentifierUpdateRequest criterias)
        {
            try
            {
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

        public async Task<UserTokenModel> UpdatePasswordAsync(ApplicationUser currentUser, UserPasswordUpdateRequest criterias)
        {
            try
            {
                ComparePassword(criterias);

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

        private void ComparePassword(UserPasswordUpdateRequest criterias)
        {
            if (!criterias.NewPassword.Equals(criterias.ConfirmPassword))
            {
                throw new ArgumentException($"{nameof(criterias.NewPassword)} and {nameof(criterias.ConfirmPassword)} is not the same");
            }
        }

        public async Task<UserTokenModel> LoginAsync(LoginModel criterias)
        {
            try
            {
                var result = await _loginManager.PasswordSignInAsync(criterias.Username, criterias.Password, true, true);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedAccessException();
                }

                var user = await _userManager.FindByNameAsync(criterias.Username);
                var token = await _userManager.GenerateUserTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_TOKEN_PURPOSE);
                await _userManager.AddLoginAsync(user, new UserLoginInfo(ServiceProvidersNameConst.CAMINO_API_AUTH, token, IdentitySettings.AUTHENTICATION_TOKEN_PURPOSE));

                var userIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
                return new UserTokenModel(true)
                {
                    AuthenticationToken = token,
                    UserInfo = new UserInfoModel()
                    {
                        UserIdentityId = userIdentityId,
                        DisplayName = user.DisplayName
                    }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<IList<UserInfoModel>> MapUsersResultToModelAsync(IEnumerable<UserFullResult> userResults, IList<UserPhotoResult> userPhotos)
        {
            var users = new List<UserInfoModel>();
            foreach (var userResult in userResults)
            {
                var userAvatar = userPhotos.FirstOrDefault(x => x.UserId == userResult.Id);
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
                    UserIdentityId = await _userManager.EncryptUserIdAsync(userResult.Id),
                    AvatarCode = userAvatar != null ? userAvatar.Code : null
                };

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
                StatusId = (byte)UserStatus.New,
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

        public async Task<CommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias)
        {
            try
            {
                ValidateForgotPassword(criterias);

                var user = await _userManager.FindByEmailAsync(criterias.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new CaminoApplicationException("ForgotPasswordConfirmation");
                }

                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.SetAuthenticationTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, IdentitySettings.RESET_PASSWORD_PURPOSE, resetPasswordToken);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => new CommonError()
                    {
                        Message = x.Description,
                        Code = x.Code
                    });
                    return CommonResult.Failed(errors);
                }

                await SendPasswordChangeAsync(criterias, user, resetPasswordToken);

                return CommonResult.Success();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateForgotPassword(ForgotPasswordModel criterias)
        {
            if (criterias == null)
            {
                throw new ArgumentException(nameof(criterias));
            }
        }

        private async Task SendPasswordChangeAsync(ForgotPasswordModel criterias, ApplicationUser user, string token)
        {
            var activeUserUrl = $"{_resetPasswordSettings.Url}/{criterias.Email}/{token}";
            await _emailSender.SendEmailAsync(new MailMessageRequest()
            {
                Body = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_BODY, user.DisplayName, _appSettings.ApplicationName, activeUserUrl),
                FromEmail = _resetPasswordSettings.FromEmail,
                FromName = _resetPasswordSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_SUBJECT, _appSettings.ApplicationName),
            }, EmailTextFormat.Html);
        }

        public async Task<CommonResult> ResetPasswordAsync(ResetPasswordModel criterias)
        {
            try
            {
                ValidateResetPassword(criterias);
                var user = await _userManager.FindByEmailAsync(criterias.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new UnauthorizedAccessException("ResetPasswordFailed");
                }

                var result = await _userManager.ResetPasswordAsync(user, criterias.Key, criterias.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => new CommonError()
                    {
                        Message = x.Description,
                        Code = x.Code
                    });

                    return CommonResult.Failed(errors);
                }
                await _userManager.RemoveAuthenticationTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, criterias.Key);

                return CommonResult.Success();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateResetPassword(ResetPasswordModel criterias)
        {
            if (criterias == null)
            {
                throw new ArgumentException(nameof(criterias));
            }

            if (!criterias.Password.Equals(criterias.ConfirmPassword))
            {
                throw new ArgumentException($"{nameof(criterias.Password)} and {nameof(criterias.ConfirmPassword)} is not the same");
            }
        }
    }
}
