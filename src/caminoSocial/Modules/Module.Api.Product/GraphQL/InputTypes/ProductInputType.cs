using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.InputTypes
{
    public class ProductInputType : InputObjectType<ProductModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductModel> descriptor)
        {
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.Id).Ignore();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.ProductCategories).Type<ListType<ProductCategoryProductInputType>>();
            descriptor.Field(x => x.Thumbnails).Type<ListType<PictureLoadInputType>>();
            descriptor.Field(x => x.UpdatedById).Ignore();
            descriptor.Field(x => x.UpdatedDate).Ignore();
            descriptor.Field(x => x.CreatedById).Ignore();
            descriptor.Field(x => x.CreatedDate).Ignore();
            descriptor.Field(x => x.CreatedBy).Ignore();
            descriptor.Field(x => x.UpdatedBy).Ignore();
            descriptor.Field(x => x.ProductCategoryName).Ignore();
            descriptor.Field(x => x.ProductCategoryId).Ignore();
        }
    }
}
