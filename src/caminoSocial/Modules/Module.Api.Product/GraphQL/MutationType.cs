using Module.Api.Product.GraphQL.InputTypes;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Camino.Core.Modular.Contracts;
using Module.Api.Product.GraphQL.ResultTypes;
using Camino.Framework.GraphQL.InputTypes;

namespace Module.Api.Product.GraphQL
{
    public class MutationType : BaseMutationType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IProductResolver>(x => x.CreateProductAsync(default, default))
               .Type<ProductResultType>()
               .Directive<AuthenticationDirectiveType>()
               .Argument("criterias", a => a.Type<ProductInputType>());

            descriptor.Field<IProductCategoryResolver>(x => x.GetProductCategoriesAsync(default))
                .Type<ListType<SelectOptionType>>()
                .Argument("criterias", a => a.Type<SelectFilterInputType>());
        }
    }
}
