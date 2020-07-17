using Coco.Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace  Coco.Api.Auth.GraphQLTypes.ResultTypes
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
