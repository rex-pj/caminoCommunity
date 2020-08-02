using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQL.ResultTypes
{
    public class CountryListType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("CountryList");
            descriptor
                .Field("countrySelections")
                .Type<CountryResultType>()
                .Resolver(ctx => ctx.Service<ICountryResolver>().GetAll());
        }
    }
}
