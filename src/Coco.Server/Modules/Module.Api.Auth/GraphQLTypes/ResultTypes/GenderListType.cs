using Module.Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQLTypes.ResultTypes
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
