using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.ResultTypes
{
    public class ProductResultType : ObjectType<ProductModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.CreatedByIdentityId).Type<StringType>();
            descriptor.Field(x => x.ProductCategoryId).Type<LongType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.UpdatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedById).Type<LongType>();
            descriptor.Field(x => x.CreatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedBy).Type<StringType>();
            descriptor.Field(x => x.UpdatedBy).Type<StringType>();
            descriptor.Field(x => x.ProductCategoryName).Type<StringType>();
            descriptor.Field(x => x.CreatedByPhotoCode).Type<StringType>();
        }
    }
}
