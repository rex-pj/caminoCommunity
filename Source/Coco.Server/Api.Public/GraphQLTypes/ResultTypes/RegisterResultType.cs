using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class RegisterResultType: ObjectType<ApiResult>
    {
        protected override void Configure(IObjectTypeDescriptor<ApiResult> descriptor)
        {
            descriptor.Field(x => x.IsSucceed).Type<BooleanType>();
            descriptor.Field(x => x.Errors).Type<ListType<ApiErrorType>>();
        }
    }
}
