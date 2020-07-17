using Coco.Api.Auth.Models;
using HotChocolate.Types;

namespace  Coco.Api.Auth.GraphQLTypes.InputTypes
{
    public class ActiveUserInputType : InputObjectType<ActiveUserModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ActiveUserModel> descriptor)
        {
            descriptor.Field(t => t.Email).Type<NonNullType<StringType>>();
            descriptor.Field(t => t.ActiveKey).Type<NonNullType<IdType>>();
        }
    }
}
