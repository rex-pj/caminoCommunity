using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using Api.Auth.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.Models;
using Coco.Common.Const;
using GraphQL;
using GraphQL.Types;
using System;
using System.Linq;

namespace Api.Auth.GraphQLQueries
{
    public class AccountQuery : ObjectGraphType
    {
        public AccountQuery(ILoginManager<ApplicationUser> loginManager)
        {
            FieldAsync(typeof(SigninResultType), "signin",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>> { Name = "signinModel" }),
                resolve: async context =>
                {
                    try
                    {
                        var model = context.GetArgument<SigninModel>("signinModel");

                        var result = await loginManager.LoginAsync(model.Username, model.Password);

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
                });
        }
    }
}
