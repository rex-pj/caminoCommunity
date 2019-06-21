using Coco.Api.Framework.AccountIdentity.Entities;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class IdentityResultType: ObjectGraphType<IdentityResult>
    {
        public IdentityResultType()
        {
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
            Field(x => x.Errors, type: typeof(ListGraphType<IdentityErrorType>));
        }
    }
}
