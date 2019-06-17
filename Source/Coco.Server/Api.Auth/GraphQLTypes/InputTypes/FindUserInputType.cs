using Api.Auth.Models;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.InputTypes
{
    public class FindUserInputType : InputObjectGraphType<FindUserModel>
    {
        public FindUserInputType()
        {
            Field(x => x.UserId, false, typeof(StringGraphType));
        }
    }
}
