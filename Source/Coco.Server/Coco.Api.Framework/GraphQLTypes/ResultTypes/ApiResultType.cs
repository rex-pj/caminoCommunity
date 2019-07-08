using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class ApiResultType : ObjectGraphType<ApiResult>
    {
        public ApiResultType()
        {
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
            Field(x => x.AccessMode, type: typeof(AccessModeEnumType));
            Field(x => x.Errors, type: typeof(ListGraphType<ApiErrorType>));
        }
    }

    public class ApiResultType<T, TResult> : ObjectGraphType<ApiResult<TResult>> where T : ObjectGraphType<TResult>
    {
        public ApiResultType()
        {
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
            Field(x => x.AccessMode, type: typeof(AccessModeEnumType));
            Field(x => x.Errors, type: typeof(ListGraphType<ApiErrorType>));
            Field(x => x.Result, type: typeof(T));
        }
    }
}
