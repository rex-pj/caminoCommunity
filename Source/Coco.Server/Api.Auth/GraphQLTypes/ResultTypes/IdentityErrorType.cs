using Coco.Api.Framework.AccountIdentity.Entities;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class IdentityErrorType : ObjectGraphType<IdentityError>
    {
        public IdentityErrorType()
        {
            Field(x => x.Code, type: typeof(StringGraphType));
            Field(x => x.Description, type: typeof(StringGraphType));
        }
    }
}
