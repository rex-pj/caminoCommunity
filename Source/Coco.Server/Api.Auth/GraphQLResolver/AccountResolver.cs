using Api.Auth.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Attributes;
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

        public AccountResolver(ILoginManager<ApplicationUser> loginManager)
        {
            _loginManager = loginManager;
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

        [AuthorizeLoggedUser]
        public async Task<LoginResult> GetLoggedUser(ResolveFieldContext<object> context)
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
    }
}
