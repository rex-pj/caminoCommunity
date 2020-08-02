using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQL.ResultTypes
{
    public class GenderListType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("GenderList");
            descriptor
                .Field("genderSelections")
                .Type<SelectOptionType>()
                .Resolver(ctx => ctx.Service<IGenderResolver>().GetSelections());
        }
    }
}
