using Api.Identity.Models;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class FindUserInputType : InputObjectGraphType<FindUserModel>
    {
        public FindUserInputType()
        {
            Field(x => x.UserId, false, typeof(StringGraphType));
        }
    }
}
