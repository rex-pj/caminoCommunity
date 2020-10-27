using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.InputTypes
{
    public class ProductFarmInputType : InputObjectType<ProductFarmModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductFarmModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.FarmId).Type<LongType>();
            descriptor.Field(x => x.FarmName).Ignore();
        }
    }
}
