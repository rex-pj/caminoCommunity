using HotChocolate.Types;
using Module.Api.Farm.Models;

namespace Module.Api.Farm.GraphQL.InputTypes
{
    public class FarmFilterInputType : InputObjectType<FarmFilterModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<FarmFilterModel> descriptor)
        {
            descriptor.Field(x => x.Page).Type<IntType>().DefaultValue(1);
            descriptor.Field(x => x.PageSize).Type<IntType>().DefaultValue(10);
            descriptor.Field(x => x.Search).Type<StringType>();
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.UserIdentityId).Type<StringType>();
        }
    }
}
