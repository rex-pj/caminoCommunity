using Camino.Framework.GraphQL.InputTypes;
using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.InputTypes
{
    public class ProductInputType : InputObjectType<ProductModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductModel> descriptor)
        {
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.Price).Type<IntType>();
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.ProductCategories).Type<ListType<ProductCategoryRelationInputType>>();
            descriptor.Field(x => x.ProductFarms).Type<ListType<ProductFarmInputType>>();
            descriptor.Field(x => x.Thumbnails).Type<ListType<PictureRequestInputType>>();
            descriptor.Field(x => x.UpdatedById).Ignore();
            descriptor.Field(x => x.UpdatedDate).Ignore();
            descriptor.Field(x => x.CreatedById).Ignore();
            descriptor.Field(x => x.CreatedDate).Ignore();
            descriptor.Field(x => x.CreatedBy).Ignore();
            descriptor.Field(x => x.UpdatedBy).Ignore();
        }
    }
}
