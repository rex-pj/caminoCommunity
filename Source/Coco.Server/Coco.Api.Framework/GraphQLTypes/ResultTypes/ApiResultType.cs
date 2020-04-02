using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class ApiResultType : ObjectType<ApiResult>
    {
        protected override void Configure(IObjectTypeDescriptor<ApiResult> descriptor)
        {
            descriptor.Field(x => x.IsSucceed).Type<BooleanType>();
            descriptor.Field(x => x.AccessMode).Type<AccessModeEnumType>();
            descriptor.Field(x => x.Errors).Type<ListType<ApiErrorType>>();
        }
    }

    public class ApiResultType<T, TResult> : ObjectType<ApiResult<T>> where T : class where TResult : class, IOutputType
    {
        protected override void Configure(IObjectTypeDescriptor<ApiResult<T>> descriptor)
        {
            descriptor.Field(x => x.IsSucceed).Type<BooleanType>();
            descriptor.Field(x => x.AccessMode).Type<AccessModeEnumType>();
            descriptor.Field(x => x.Errors).Type<ListType<ApiErrorType>>();
            descriptor.Field(x => x.Result).Type<TResult>();
        }
    }
}
