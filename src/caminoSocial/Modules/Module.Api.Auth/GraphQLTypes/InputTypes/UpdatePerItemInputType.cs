using Camino.Framework.Models;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQLTypes.InputTypes
{
    public class UpdatePerItemInputType : InputObjectType<UpdatePerItemModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePerItemModel> descriptor)
        {
            descriptor.Field(x => x.Key).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.PropertyName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Value).Type<AnyType>();
            descriptor.Field(x => x.CanEdit).Type<NonNullType<BooleanType>>();
        }
    }
}
