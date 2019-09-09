using Api.Public.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.InputTypes
{
    public class ForgotPasswordInputType: InputObjectGraphType<ForgotPasswordModel>
    {
        public ForgotPasswordInputType()
        {
            Field(x => x.Email, false, typeof(StringGraphType));
        }
    }
}
