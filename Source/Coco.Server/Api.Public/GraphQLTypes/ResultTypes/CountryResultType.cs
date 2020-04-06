using Api.Public.Models;
using HotChocolate.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class CountryResultType : ObjectType<CountryModel>
    {
        protected override void Configure(IObjectTypeDescriptor<CountryModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<ShortType>();
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.Code).Type<StringType>();
        }
    }
}
