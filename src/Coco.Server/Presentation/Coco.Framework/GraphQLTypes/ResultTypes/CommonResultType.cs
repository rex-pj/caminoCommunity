using Coco.Framework.Models;
using HotChocolate.Types;

namespace Coco.Framework.GraphQLTypes.ResultTypes
{
    public class CommonResultType : ObjectType<ICommonResult>
    {
        protected override void Configure(IObjectTypeDescriptor<ICommonResult> descriptor)
        {
            descriptor.Field(x => x.IsSucceed).Type<BooleanType>();
            descriptor.Field(x => x.AccessMode).Type<AccessModeEnumType>();
            descriptor.Field(x => x.Errors).Type<ListType<CommonErrorType>>();
            descriptor.Field(x => x.Result).Type<AnyType>();
        }
    }
}
