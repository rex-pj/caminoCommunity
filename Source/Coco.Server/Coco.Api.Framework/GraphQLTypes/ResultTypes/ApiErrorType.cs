using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class ApiErrorType : ObjectType<ApiError>
    {
        protected override void Configure(IObjectTypeDescriptor<ApiError> descriptor)
        {
            descriptor.Field(x => x.Code).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
        }
    }
}
