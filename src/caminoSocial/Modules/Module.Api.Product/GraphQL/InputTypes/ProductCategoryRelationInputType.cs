using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.InputTypes
{
    public class ProductCategoryRelationInputType : InputObjectType<ProductCategoryRelationModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductCategoryRelationModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<IntType>();
        }
    }
}
