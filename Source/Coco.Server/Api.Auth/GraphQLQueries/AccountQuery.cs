using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using Api.Auth.Models;
using Coco.Api.Framework.Models;
using Coco.Common.Const;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using System;

namespace Api.Auth.GraphQLQueries
{
    public class AccountQuery : ObjectGraphType
    {
        public AccountQuery(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager)
        {
            FieldAsync(typeof(SigninResultType), "signin",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>> { Name = "signinModel" }),
                resolve: async context =>
                {
                    try
                    {
                        var model = context.GetArgument<SigninModel>("signinModel");

                        var signinResult = await signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);


                        return new SigninResultModel() {
                            
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
                    }
                });
        }
    }
}
