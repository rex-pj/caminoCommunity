using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class ActiveUserResultType : ObjectGraphType<ApiResult>
    {
        public ActiveUserResultType()
        {
            Field(x => x.IsSucceed, type: typeof(BooleanGraphType));
            Field(x => x.AccessMode, type: typeof(AccessModeEnumType));
            Field(x => x.Errors, type: typeof(ListGraphType<ApiErrorType>));
        }
    }
}
