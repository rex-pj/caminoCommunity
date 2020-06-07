using Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class GenderListType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("GenderList");
            descriptor
                .Field("GenderSelections")
                .Type<SelectOptionType>()
                .Resolver(ctx => ctx.Service<IGenderResolver>().GetSelections());
        }
    }
}
