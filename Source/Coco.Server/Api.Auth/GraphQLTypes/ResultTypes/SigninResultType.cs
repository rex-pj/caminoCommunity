using Api.Auth.Models;
using Coco.Api.Framework.AccountIdentity.Entities;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class SigninResultType: ObjectGraphType<LoginResult>
    {
        public SigninResultType()
        {
            Field(x => x.AuthenticatorToken, type: typeof(StringGraphType));
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
            Field(x => x.Errors, type: typeof(ListGraphType<IdentityErrorType>));
        }
    }
}
