using Api.Public.Models;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.Resolvers;
using Coco.Entities.Enums;
using System;
using System.Threading.Tasks;
using Coco.Api.Framework.Services.Contracts;
using MimeKit.Text;
using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Common.Resources;
using Microsoft.Extensions.Configuration;
using Api.Public.Resolvers.Contracts;
using HotChocolate.Resolvers;

namespace Api.Public.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly string _appName;
        private readonly string _registerConfirmUrl;
        private readonly string _resetPasswordUrl;
        private readonly string _registerConfirmFromEmail;
        private readonly string _registerConfirmFromName;

        public UserResolver(ILoginManager<ApplicationUser> loginManager,
            IUserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _loginManager = loginManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _appName = configuration["ApplicationName"];
            _registerConfirmUrl = configuration["RegisterConfimation:Url"];
            _registerConfirmFromEmail = configuration["RegisterConfimation:FromEmail"];
            _registerConfirmFromName = configuration["RegisterConfimation:FromName"];
            _resetPasswordUrl = configuration["ResetPassword:Url"];
        }

        public async Task<UserTokenResult> SigninAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<SigninModel>("criterias");
                var result = await _loginManager.LoginAsync(model.Username, model.Password);
                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

                return result.Result as UserTokenResult;
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<IApiResult> SignupAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<SignupModel>("criterias");
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
                    var response = result as ApiResult;
                    var userResponse = response.Result as ApplicationUser;
                    var activeUserUrl = $"{_registerConfirmUrl}/{user.Email}/{userResponse.ActiveUserStamp}";
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
                throw ex;
            }
        }

        public async Task<IApiResult> ForgotPasswordAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<ForgotPasswordModel>("criterias");
                if (criterias == null)
                {
                    throw new NullReferenceException(nameof(criterias));
                }

                var user = await _userManager.FindByEmailAsync(criterias.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    throw new ApplicationException("ForgotPasswordConfirmation");
                }

                var result = await _userManager.ForgotPasswordAsync(criterias.Email);

                var response = result as ApiResult;
                var userResponse = response.Result.ToString();
                var activeUserUrl = $"{_resetPasswordUrl}/{criterias.Email}/{userResponse}";
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
                throw ex;
            }
        }

        public async Task<IApiResult> ActiveAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<ActiveUserModel>("criterias");

                var result = await _userManager.ActiveAsync(model.Email, model.ActiveKey);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserTokenResult> ResetPasswordAsync(IResolverContext context)
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

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
