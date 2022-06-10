using Camino.Framework.Models;
using Camino.Framework.GraphQL.Resolvers;
using System;
using System.Threading.Tasks;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Infrastructure.Emails.Contracts;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Application.Contracts;
using Camino.Shared.Commons;
using Camino.Infrastructure.Emails.Contracts.Dtos;
using Camino.Infrastructure.Emails.Templates;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IUserAppService _userAppService;
        private readonly IUserPhotoAppService _userPhotoAppService;
        private readonly IEmailClient _emailClient;
        private readonly AppSettings _appSettings;
        private readonly RegisterConfirmationSettings _registerConfirmationSettings;
        private readonly PagerOptions _pagerOptions;

        public UserResolver(IUserManager<ApplicationUser> userManager, IEmailClient emailClient,
            IUserAppService userAppService, IOptions<AppSettings> appSettings,
            IUserPhotoAppService userPhotoAppService, IOptions<RegisterConfirmationSettings> registerConfirmationSettings, IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _userManager = userManager;
            _userAppService = userAppService;
            _appSettings = appSettings.Value;
            _registerConfirmationSettings = registerConfirmationSettings.Value;
            _emailClient = emailClient;
            _userPhotoAppService = userPhotoAppService;
            _pagerOptions = pagerOptions.Value;
        }

        #region Get
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
                var userPageList = await _userAppService.GetAsync(filterRequest);
                var userIds = userPageList.Collections.Select(x => x.Id);
                var userPhotos = await _userPhotoAppService.GetListByUserIdsAsync(userIds, UserPictureTypes.Avatar);
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

            var data = await _userAppService.SearchAsync(new UserFilter
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

            var user = await _userAppService.FindFullByIdAsync(new IdRequestFilter<long>
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
        #endregion

        #region CRUD
        public async Task<PartialUpdateResultModel> PartialUserUpdateAsync(ClaimsPrincipal claimsPrincipal, PartialUpdateRequestModel criterias)
        {
            try
            {
                ValidatePartialUser(criterias);
                var currentUserId = GetCurrentUserId(claimsPrincipal);
                var userId = await _userManager.DecryptUserIdAsync(criterias.Key.ToString());
                if (userId != currentUserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var updatedItem = await _userAppService.PartialUpdateAsync(new PartialUpdateRequest
                {
                    Key = userId,
                    PropertyName = criterias.PropertyName,
                    Value = criterias.Value
                });

                return new PartialUpdateResultModel
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

        private void ValidatePartialUser(PartialUpdateRequestModel criterias)
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

        private async Task<IList<UserInfoModel>> MapUsersResultToModelAsync(IEnumerable<UserFullResult> userResults, IList<UserPhotoResult> userPhotos = null)
        {
            var users = new List<UserInfoModel>();
            foreach (var userResult in userResults)
            {
                var user = new UserInfoModel
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
                StatusId = (byte)UserStatuses.Pending,
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
            await _emailClient.SendEmailAsync(new MailMessageRequest()
            {
                Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appSettings.ApplicationName, activeUserUrl),
                FromEmail = _registerConfirmationSettings.FromEmail,
                FromName = _registerConfirmationSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _appSettings.ApplicationName),
            }, EmailTextFormats.Html);
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
