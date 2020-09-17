using Camino.Core.Modular.Contracts;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Module.Api.Content.GraphQL.InputTypes;
using Module.Api.Content.GraphQL.Resolvers.Contracts;

namespace Module.Api.Content.GraphQL
{
    public class QueryType : BaseQueryType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IArticleCategoryResolver>(x => x.GetCategories(default))
                .Type<ListType<SelectOptionType>>()
                .Argument("criterias", a => a.Type<SelectFilterInputType>());
        }
    }
}
