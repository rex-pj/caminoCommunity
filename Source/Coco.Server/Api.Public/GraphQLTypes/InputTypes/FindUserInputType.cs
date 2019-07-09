using Api.Public.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.InputTypes
{
    public class FindUserInputType : InputObjectGraphType<FindUserModel>
    {
        public FindUserInputType()
        {
            Field(x => x.UserId, false, typeof(StringGraphType));
        }
    }
}
