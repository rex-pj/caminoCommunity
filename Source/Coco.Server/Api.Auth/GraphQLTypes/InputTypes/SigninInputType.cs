using Api.Auth.Models;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.InputTypes
{
    public class SigninInputType : InputObjectGraphType<SigninModel>
    {
        public SigninInputType()
        {
            Field(x => x.Password, type: typeof(StringGraphType));
            Field(x => x.Username, type: typeof(StringGraphType));
        }
    }
}
