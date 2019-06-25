using Api.Auth.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.Commons.Encode;
using Coco.Api.Framework.Commons.Helpers;
using Coco.Api.Framework.Mapping;
using Coco.Api.Framework.Models;
using Coco.Common.Const;
using Coco.Entities.Enums;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Auth.GraphQLResolver
{
    public class AccountResolver
    {
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IAccountManager<ApplicationUser> _accountManager;

        public AccountResolver(ILoginManager<ApplicationUser> loginManager,
            IAccountManager<ApplicationUser> accountManager)
        {
            _loginManager = loginManager;
            _accountManager = accountManager;
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

        public async Task<UserInfo> GetLoggedUserAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var userHeaderParams = HttpHelper.GetAuthorizationHeaders(context);

                var result = await _accountManager.GetLoggingUser(userHeaderParams.UserIdHashed, userHeaderParams.AuthenticationToken);
                if(result == null)
                {
                    return new UserInfo();
                }

                return UserInfoMapping.ApplicationUserToUserInfo(result, userHeaderParams.UserIdHashed);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserInfo> GetFullUserInfoAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<FindUserModel>("criterias");

                string userIdHased = model.UserId;
                if (string.IsNullOrEmpty(model.UserId))
                {
                    var userHeaderParams = HttpHelper.GetAuthorizationHeaders(context);
                    userIdHased = userHeaderParams.UserIdHashed;
                }

                var result = await _accountManager.GetFullByHashIdAsync(userIdHased);

                return UserInfoMapping.ApplicationUserToFullUserInfo(result, userIdHased);
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

                var parameters = new ApplicationUser()
                {
                    BirthDate = model.BirthDate,
                    CreatedDate = DateTime.Now,
                    DisplayName = $"{model.Lastname} {model.Firstname}",
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    GenderId = (byte)model.GenderId,
                    StatusId = (byte)UserStatusEnum.Pending,
                    UpdatedDate = DateTime.Now,
                    UserName = model.Email,
                    PasswordSalt = SaltGenerator.GetSalt(),
                    SecurityStamp = SaltGenerator.GetSalt(),
                    Password = model.Password
                };

                var result = await _accountManager.CreateAsync(parameters);

                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> UpdateUserInfoAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<UserInfoUpdateModel>("user");

                var parameters = new ApplicationUser()
                {
                    BirthDate = model.BirthDate,
                    DisplayName = $"{model.Lastname} {model.Firstname}",
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    GenderId = (byte)model.GenderId,
                    UpdatedDate = DateTime.Now,
                    UserName = model.Email,
                    Description = model.Description,
                    Address = model.Address,
                    CountryId = model.CountryId,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _accountManager.UpdateInfoAsync(parameters);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> UpdateUserInfoItemAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<UpdatePerItemModel>("criterias");
                var userHeaderParams = HttpHelper.GetAuthorizationHeaders(context);

                var result = await _accountManager.UpdateInfoItemAsync(model, userHeaderParams.AuthenticationToken);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        private void HandleContextError(ResolveFieldContext<object> context, IEnumerable<ApiError> errors)
        {
            if (errors != null && errors.Any())
            {
                foreach (var error in errors)
                {
                    if (!context.Errors.Any() || !context.Errors.Any(x => error.Code.Equals(x.Code)))
                    {
                        context.Errors.Add(new ExecutionError(error.Description)
                        {
                            Code = error.Code
                        });
                    }
                }
            }
        }
    }
}
