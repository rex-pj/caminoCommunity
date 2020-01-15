using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class SignoutResultType : ObjectGraphType<ApiResult>
    {
        public SignoutResultType()
        {
            Field(x => x.IsSucceed, type: typeof(BooleanGraphType));
            Field(x => x.AccessMode, type: typeof(AccessModeEnumType));
            Field(x => x.Errors, type: typeof(ListGraphType<ApiErrorType>));
        }
    }
}
