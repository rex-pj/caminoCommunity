using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.ResultTypes
{
    public class ProductPageListType : ObjectType<ProductPageListModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductPageListModel> descriptor)
        {
            descriptor.Field(x => x.Collections).Type<ListType<ProductResultType>>();
            descriptor.Field(x => x.TotalPage).Type<IntType>();
            descriptor.Field(x => x.TotalResult).Type<IntType>();
            descriptor.Field(x => x.Filter).Type<BaseFilterResultType>();
        }
    }
}
