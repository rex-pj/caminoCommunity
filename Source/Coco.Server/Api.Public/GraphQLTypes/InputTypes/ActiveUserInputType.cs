using Api.Public.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.InputTypes
{
    public class ActiveUserInputType : InputObjectGraphType<ActiveUserModel>
    {
        public ActiveUserInputType()
        {
            Field(x => x.Email, false, typeof(StringGraphType));
            Field(x => x.ActiveKey, false, typeof(StringGraphType));
        }
    }
}
