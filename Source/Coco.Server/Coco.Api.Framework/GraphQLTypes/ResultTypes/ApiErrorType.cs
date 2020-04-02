using Coco.Commons.Models;
using HotChocolate.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class ApiErrorType : ObjectType<CommonError>
    {
        protected override void Configure(IObjectTypeDescriptor<CommonError> descriptor)
        {
            descriptor.Field(x => x.Code).Type<StringType>();
            descriptor.Field(x => x.Message).Type<StringType>();
        }
    }
}
