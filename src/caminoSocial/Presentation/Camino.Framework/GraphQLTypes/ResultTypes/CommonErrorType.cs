using Camino.Core.Models;
using HotChocolate.Types;

namespace Camino.Framework.GraphQLTypes.ResultTypes
{
    public class CommonErrorType : ObjectType<CommonError>
    {
        protected override void Configure(IObjectTypeDescriptor<CommonError> descriptor)
        {
            descriptor.Field(x => x.Code).Type<StringType>();
            descriptor.Field(x => x.Message).Type<StringType>();
        }
    }
}
