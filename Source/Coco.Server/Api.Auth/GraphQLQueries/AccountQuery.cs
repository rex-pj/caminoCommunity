using Api.Auth.GraphQLResolver;
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
        public AccountQuery(AccountResolver accountResolver)
        {
            FieldAsync(typeof(SigninResultType), "signin",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>> { Name = "signinModel" }),
                resolve: async context => await accountResolver.Signin(context));

            FieldAsync(typeof(SigninResultType), "getLoggedUser",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SigninInputType>>()),
                resolve: async context => await accountResolver.Signin(context));
        }
    }
}
