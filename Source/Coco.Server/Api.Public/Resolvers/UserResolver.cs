using Api.Public.Models;
using Coco.Api.Framework.Commons.Helpers;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.Resolvers;
using Coco.Business.Contracts;
using Coco.Common.Const;
using Coco.Entities.Enums;
using GraphQL;
using GraphQL.Types;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Coco.Api.Framework.Services.Contracts;
using MimeKit.Text;
using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Common.Resources;
using Microsoft.Extensions.Configuration;

namespace Api.Public.Resolvers
{
    public class UserResolver : BaseResolver
    {
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ICountryBusiness _countryBusiness;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly string _appName;
        private readonly string _registerConfirmUrl;
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
        }

        public async Task<ApiResult> SigninAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<SigninModel>("signinModel");

                var result = await _loginManager.LoginAsync(model.Username, model.Password);

                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> SignupAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<RegisterModel>("user");

                var user = new ApplicationUser()
                {
                    BirthDate = model.BirthDate,
                    CreatedDate = DateTime.Now,
                    DisplayName = $"{model.Lastname} {model.Firstname}",
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    GenderId = (byte)model.GenderId,
                    StatusId = (byte)UserStatusEnum.New,
                    UpdatedDate = DateTime.Now,
                    UserName = model.Email,
                    Password = model.Password
                };

                var result = await _userManager.CreateAsync(user);
                if (result.IsSuccess)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _emailSender.SendEmailAsync(new MailMessageModel()
                    {
                        Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appName, _registerConfirmUrl),
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
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> GetFullUserInfoAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<FindUserModel>("criterias");

                var userIdentityId = model.UserId;

                var userContext = context.UserContext as ISessionContext;
                if (string.IsNullOrEmpty(model.UserId) && userContext != null
                    && userContext.CurrentUser != null)
                {
                    userIdentityId = userContext.CurrentUser.UserIdentityId;
                }

                var user = await _userManager.GetFullByHashIdAsync(userIdentityId);

                var result = _mapper.Map<UserInfoExt>(user);
                result.UserIdentityId = userIdentityId;
                if (userContext.CurrentUser == null || !user.AuthenticationToken.Equals(userContext.AuthenticationToken))
                {
                    return ApiResult<UserInfoExt>.Success(result);
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

                return ApiResult<UserInfoExt>.Success(result, true);
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ex.ToString(), ex);
            }
        }

        public async Task<ApiResult> ForgotPasswordAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<ForgotPasswordModel>("criterias");

                if (model == null)
                {
                    throw new NullReferenceException(nameof(model));
                }

                var user = await _userManager.FindByEmailAsync(model.Email, true);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new ExecutionError("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ForgotPasswordAsync(model.Email);
                
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code });
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");

                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }
    }
}
