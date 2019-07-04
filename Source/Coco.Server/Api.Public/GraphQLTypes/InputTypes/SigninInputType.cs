using Api.Public.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.InputTypes
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
