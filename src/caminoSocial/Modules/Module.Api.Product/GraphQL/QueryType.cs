using Camino.Core.Modular.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using HotChocolate.Types;
using Module.Api.Product.GraphQL.InputTypes;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Module.Api.Product.GraphQL.ResultTypes;

namespace Module.Api.Product.GraphQL
{
    public class QueryType : BaseQueryType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IProductResolver>(x => x.GetUserProductsAsync(default))
                .Type<ProductPageListType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<ProductFilterInputType>());

            descriptor.Field<IProductResolver>(x => x.GetProductsAsync(default))
                .Type<ProductPageListType>()
                .Argument("criterias", a => a.Type<ProductFilterInputType>());

            descriptor.Field<IProductResolver>(x => x.GetProductAsync(default))
                .Type<ProductResultType>()
                .Argument("criterias", a => a.Type<ProductFilterInputType>());
        }
    }
}
