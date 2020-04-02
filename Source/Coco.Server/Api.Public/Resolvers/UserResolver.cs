using Api.Public.Models;
using Coco.Api.Framework.Commons.Helpers;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.Resolvers;
using Coco.Business.Contracts;
using Coco.Entities.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Coco.Api.Framework.Services.Contracts;
using MimeKit.Text;
using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Common.Resources;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Api.Public.Resolvers.Contracts;
using HotChocolate.Resolvers;

namespace Api.Public.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ICountryBusiness _countryBusiness;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly string _appName;
        private readonly string _registerConfirmUrl;
        private readonly string _resetPasswordUrl;
        private readonly string _registerConfirmFromEmail;
        private readonly string _registerConfirmFromName;

        public UserResolver(ILoginManager<ApplicationUser> loginManager,
            IUserManager<ApplicationUser> userManager,
            IMapper mapper,
            IEmailSender emailSender,
            IConfiguration configuration,
            ICountryBusiness countryBusiness)
        {
            _loginManager = loginManager;
            _userManager = userManager;
            _countryBusiness = countryBusiness;
            _mapper = mapper;
            _emailSender = emailSender;
            _appName = configuration["ApplicationName"];
            _registerConfirmUrl = configuration["RegisterConfimation:Url"];
            _registerConfirmFromEmail = configuration["RegisterConfimation:FromEmail"];
            _registerConfirmFromName = configuration["RegisterConfimation:FromName"];
            _resetPasswordUrl = configuration["ResetPassword:Url"];
        }

        public ApplicationUser GetLoggedUser(IDictionary<string, object> userContext)
        {
            try
            {
                var sessionContext = userContext["SessionContext"] as ISessionContext;
                var currentUser = sessionContext.CurrentUser;
                return currentUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResult> SigninAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<SigninModel>("args");

                var result = await _loginManager.LoginAsync(model.Username, model.Password);

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                //context.ReportError(ErrorMessageConst.EXCEPTION);
                 throw;
            }
        }

        public async Task<ApiResult> SignupAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<RegisterModel>("user");

                var user = new ApplicationUser()
                {
                    BirthDate = model.BirthDate,
                    CreatedDate = DateTime.UtcNow,
                    DisplayName = $"{model.Lastname} {model.Firstname}",
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    GenderId = (byte)model.GenderId,
                    StatusId = (byte)UserStatusEnum.New,
                    UpdatedDate = DateTime.UtcNow,
                    UserName = model.Email,
                    Password = model.Password
                };

                var result = await _userManager.CreateAsync(user);
                if (result.IsSucceed)
                {
                    var response = result as ApiResult<ApplicationUser>;
                    var activeUserUrl = $"{_registerConfirmUrl}/{user.Email}/{response.Result.ActiveUserStamp}";
                    await _emailSender.SendEmailAsync(new MailMessageModel()
                    {
                        Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appName, activeUserUrl),
                        FromEmail = _registerConfirmFromEmail,
                        FromName = _registerConfirmFromName,
                        ToEmail = user.Email,
                        ToName = user.DisplayName,
                        Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _appName),
                    }, TextFormat.Html);
                }
                else
                {
                    HandleContextError(context, result.Errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiResult> GetFullUserInfoAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<FindUserModel>("criterias");
                var userIdentityId = model.UserId;

                var userContext = context.ContextData["SessionContext"] as ISessionContext;
                if (string.IsNullOrEmpty(model.UserId) && userContext != null
                    && userContext.CurrentUser != null)
                {
                    userIdentityId = userContext.CurrentUser.UserIdentityId;
                }

                var user = await _userManager.FindUserByIdentityIdAsync(userIdentityId, userContext.AuthenticationToken);

                var result = _mapper.Map<UserInfoExtend>(user);
                result.UserIdentityId = userIdentityId;
                if (userContext.CurrentUser == null || string.IsNullOrEmpty(user.AuthenticationToken) 
                    || !user.AuthenticationToken.Equals(userContext.AuthenticationToken))
                {
                    return ApiResult<UserInfoExtend>.Success(result);
                }

                var genderOptions = EnumHelper.EnumToSelectList<GenderEnum>();

                result.GenderSelections = genderOptions;
                var countries = _countryBusiness.GetAll();
                if (countries != null && countries.Any())
                {
                    result.CountrySelections = countries.Select(x => new SelectOption()
                    {
                        Id = x.Id.ToString(),
                        Text = x.Name
                    });
                }

                return ApiResult<UserInfoExtend>.Success(result, true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiResult> ForgotPasswordAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<ForgotPasswordModel>("criterias");

                if (model == null)
                {
                    throw new NullReferenceException(nameof(model));
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new ApplicationException("ForgotPasswordConfirmation");
                }

                var result = await _userManager.ForgotPasswordAsync(model.Email);

                var response = result as ApiResult<string>;
                var activeUserUrl = $"{_resetPasswordUrl}/{model.Email}/{response.Result}";
                await _emailSender.SendEmailAsync(new MailMessageModel()
                {
                    Body = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_BODY, user.DisplayName, _appName, activeUserUrl),
                    FromEmail = _registerConfirmFromEmail,
                    FromName = _registerConfirmFromName,
                    ToEmail = user.Email,
                    ToName = user.DisplayName,
                    Subject = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_SUBJECT, _appName),
                }, TextFormat.Html);

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiResult> ActiveAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<ActiveUserModel>("criterias");

                var result = await _userManager.ActiveAsync(model.Email, model.ActiveKey);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiResult> ResetPasswordAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<ResetPasswordModel>("criterias");

                if (model == null)
                {
                    throw new NullReferenceException(nameof(model));
                }

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    throw new ArgumentException($"{nameof(model.Password)} and {nameof(model.ConfirmPassword)} is not the same");
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new UnauthorizedAccessException("ResetPasswordFailed");
                }

                var result = await _userManager.ResetPasswordAsync(model, user.Id);

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
