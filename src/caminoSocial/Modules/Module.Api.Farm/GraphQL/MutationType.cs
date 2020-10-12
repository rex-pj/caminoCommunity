using Module.Api.Farm.GraphQL.InputTypes;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Camino.Core.Modular.Contracts;
using Module.Api.Farm.GraphQL.ResultTypes;
using Camino.Framework.GraphQL.InputTypes;

namespace Module.Api.Farm.GraphQL
{
    public class MutationType : BaseMutationType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IFarmResolver>(x => x.CreateFarmAsync(default))
               .Type<FarmResultType>()
               .Directive<AuthenticationDirectiveType>()
               .Argument("criterias", a => a.Type<FarmInputType>());

            descriptor.Field<IFarmTypeResolver>(x => x.GetFarmTypes(default))
                .Type<ListType<SelectOptionType>>()
                .Argument("criterias", a => a.Type<SelectFilterInputType>());
        }
    }
}
