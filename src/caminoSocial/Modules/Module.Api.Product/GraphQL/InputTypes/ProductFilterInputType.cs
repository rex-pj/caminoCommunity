using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.InputTypes
{
    public class ProductFilterInputType : InputObjectType<ProductFilterModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductFilterModel> descriptor)
        {
            descriptor.Field(x => x.Page).Type<IntType>().DefaultValue(1);
            descriptor.Field(x => x.PageSize).Type<IntType>().DefaultValue(10);
            descriptor.Field(x => x.Search).Type<StringType>();
            descriptor.Field(x => x.UserIdentityId).Type<StringType>();
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.FarmId).Type<LongType>();
        }
    }
}
