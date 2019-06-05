using Api.Auth.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Attributes;
using Coco.Api.Framework.Mapping;
using Coco.Api.Framework.Models;
using Coco.Common.Const;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
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
                var httpContext = context.UserContext as DefaultHttpContext;
                var httpHeaders = httpContext.Request.Headers as HttpRequestHeaders;

                var userAuthenticationToken = httpHeaders.HeaderAuthorization;
                var userHashedIds = httpHeaders.GetCommaSeparatedValues("x-header-user-hash");
                var userHashedId = userHashedIds.FirstOrDefault();

                var result = await _accountManager.FindByTokenAsync(userHashedId, userAuthenticationToken);

                return UserInfoMapping.ApplicationUserToUserInfo(result, userHashedId);
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
                var httpContext = context.UserContext as DefaultHttpContext;
                var httpHeaders = httpContext.Request.Headers as HttpRequestHeaders;

                var userAuthenticationToken = httpHeaders.HeaderAuthorization;
                var userHashedIds = httpHeaders.GetCommaSeparatedValues("x-header-user-hash");
                var userHashedId = userHashedIds.FirstOrDefault();

                var result = await _accountManager.GetFullByTokenAsync(userHashedId, userAuthenticationToken);

                return UserInfoMapping.ApplicationUserToFullUserInfo(result, userHashedId);
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }
    }
}
