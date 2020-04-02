using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class ApiResultType : ObjectType<IApiResult>
    {
        protected override void Configure(IObjectTypeDescriptor<IApiResult> descriptor)
        {
            descriptor.Field(x => x.IsSucceed).Type<BooleanType>();
            descriptor.Field(x => x.AccessMode).Type<AccessModeEnumType>();
            descriptor.Field(x => x.Errors).Type<ListType<ApiErrorType>>();
            descriptor.Field(x => x.Result).Type<AnyType>();
        }
    }
}
