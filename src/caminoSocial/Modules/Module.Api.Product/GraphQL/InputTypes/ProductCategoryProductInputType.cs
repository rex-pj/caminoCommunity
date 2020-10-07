using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.InputTypes
{
    public class ProductCategoryProductInputType : InputObjectType<ProductCategoryProductModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductCategoryProductModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<IntType>();
        }
    }
}
