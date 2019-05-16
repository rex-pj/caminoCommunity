using Coco.Api.Framework.AccountIdentity.Entities;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class RegisterResultType: ObjectGraphType<IdentityResult>
    {
        public RegisterResultType()
        {
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
            Field(x => x.Errors, type: typeof(ListGraphType<IdentityErrorType>));
        }
    }
}
