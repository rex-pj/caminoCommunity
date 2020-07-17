using Coco.Api.Auth.Models;
using HotChocolate.Types;

namespace  Coco.Api.Auth.GraphQLTypes.InputTypes
{
    public class FindUserInputType : InputObjectType<FindUserModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<FindUserModel> descriptor)
        {
            descriptor.Field(x => x.UserId).Type<NonNullType<StringType>>();
        }
    }
}
