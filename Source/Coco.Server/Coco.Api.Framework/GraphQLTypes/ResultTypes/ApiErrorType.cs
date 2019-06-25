using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class ApiErrorType : ObjectGraphType<ApiError>
    {
        public ApiErrorType()
        {
            Field(x => x.Code, type: typeof(StringGraphType));
            Field(x => x.Description, type: typeof(StringGraphType));
        }
    }
}
