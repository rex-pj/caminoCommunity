using Api.Auth.Models;
using Coco.Auth.Models;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes.InputTypes
{
    public class FindUserInputType : InputObjectType<FindUserModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<FindUserModel> descriptor)
        {
            descriptor.Field(x => x.UserId).Type<NonNullType<StringType>>();
        }
    }
}
