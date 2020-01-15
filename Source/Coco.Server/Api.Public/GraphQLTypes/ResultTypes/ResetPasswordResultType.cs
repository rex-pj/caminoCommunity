using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class ResetPasswordResultType : ObjectGraphType<ApiResult>
    {
        public ResetPasswordResultType()
        {
            Field(x => x.IsSucceed, type: typeof(BooleanGraphType));
            Field(x => x.Errors, type: typeof(ListGraphType<ApiErrorType>));
        }
    }
}
