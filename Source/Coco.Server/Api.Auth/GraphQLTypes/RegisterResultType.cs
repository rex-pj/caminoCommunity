using GraphQL.Types;
using Microsoft.AspNetCore.Identity;

namespace Api.Auth.GraphQLTypes
{
    public class RegisterResultType: ObjectGraphType<IdentityResult>
    {
        public RegisterResultType()
        {
            Field(x => x.Succeeded, type: typeof(BooleanGraphType));
            Field(x => x.Errors, type: typeof(ListGraphType<IdentityErrorType>));
        }
    }
}
