using Camino.Core.Modular.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using HotChocolate.Types;
using Module.Api.Farm.GraphQL.InputTypes;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.GraphQL.ResultTypes;

namespace Module.Api.Farm.GraphQL
{
    public class QueryType : BaseQueryType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IFarmResolver>(x => x.GetUserFarmsAsync(default))
                .Type<FarmPageListType>()
                .Argument("criterias", a => a.Type<FarmFilterInputType>());

            descriptor.Field<IFarmResolver>(x => x.GetFarmsAsync(default))
                .Type<FarmPageListType>()
                .Argument("criterias", a => a.Type<FarmFilterInputType>());

            descriptor.Field<IFarmResolver>(x => x.GetFarmAsync(default))
                .Type<FarmResultType>()
                .Argument("criterias", a => a.Type<FarmFilterInputType>());
        }
    }
}
