using Api.Auth.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Commons.Helpers;
using Coco.Api.Framework.Mapping;
using Coco.Api.Framework.Models;
using Coco.Common.Const;
using GraphQL;
using GraphQL.Types;
using System;
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

        public async Task<LoginResult> Signin(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<SigninModel>("signinModel");

                var result = await _loginManager.LoginAsync(model.Username, model.Password);

                if (result.Errors != null && result.Errors.Any())
                {
                    foreach (var error in result.Errors)
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

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<UserInfo> GetLoggedUser(ResolveFieldContext<object> context)
        {
            try
            {
                var userHeaderParams = HttpHelper.GetAuthorizationHeaders(context);

                var result = await _accountManager.GetLoggingUser(userHeaderParams.UserIdHashed, userHeaderParams.AuthenticationToken);

                return UserInfoMapping.ApplicationUserToUserInfo(result, userHeaderParams.UserIdHashed);
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<UserInfo> GetFullLoggedUser(ResolveFieldContext<object> context)
        {
            try
            {
                var userHeaderParams = HttpHelper.GetAuthorizationHeaders(context);

                var result = await _accountManager.GetFullByTokenAsync(userHeaderParams.UserIdHashed);

                return UserInfoMapping.ApplicationUserToFullUserInfo(result, userHeaderParams.UserIdHashed);
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }
    }
}
