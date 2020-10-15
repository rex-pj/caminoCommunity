using HotChocolate.Types;
using Module.Api.Feed.Models;

namespace Module.Api.Feed.GraphQL.InputTypes
{
    public class FeedFilterInputType : InputObjectType<FeedFilterModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<FeedFilterModel> descriptor)
        {
            descriptor.Field(x => x.Page).Type<IntType>().DefaultValue(1);
            descriptor.Field(x => x.PageSize).Type<IntType>().DefaultValue(10);
            descriptor.Field(x => x.Search).Type<StringType>();
            descriptor.Field(x => x.UserIdentityId).Type<StringType>();
        }
    }
}
