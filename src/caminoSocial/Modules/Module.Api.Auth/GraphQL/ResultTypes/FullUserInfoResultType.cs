using Module.Api.Auth.Models;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQL.ResultTypes
{
    public class FullUserInfoResultType : ObjectType<FullUserInfoModel>
    {
        protected override void Configure(IObjectTypeDescriptor<FullUserInfoModel> descriptor)
        {
            descriptor.Field(x => x.Lastname).Type<StringType>();
            descriptor.Field(x => x.Firstname).Type<StringType>();
            descriptor.Field(x => x.Email).Type<StringType>();
            descriptor.Field(x => x.DisplayName).Type<StringType>();
            descriptor.Field(x => x.UserIdentityId).Type<StringType>();
            descriptor.Field(x => x.Address).Type<StringType>();
            descriptor.Field(x => x.PhoneNumber).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.BirthDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.UpdatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.GenderId).Type<IntType>();
            descriptor.Field(x => x.GenderLabel).Type<StringType>();
            descriptor.Field(x => x.CountryId).Type<ShortType>();
            descriptor.Field(x => x.CountryCode).Type<StringType>();
            descriptor.Field(x => x.CountryName).Type<StringType>();
            descriptor.Field(x => x.StatusId).Type<IntType>();
            descriptor.Field(x => x.StatusLabel).Type<StringType>();
            descriptor.Field(x => x.GenderSelections)
                .Resolver(ctx => ctx.Service<IGenderResolver>().GetSelections())
                .Type<ListType<SelectOptionType>>();
            descriptor.Field(x => x.CountrySelections)
                .Type<ListType<CountryResultType>>()
                .Resolver(ctx => ctx.Service<ICountryResolver>().GetAll());
        }
    }
}
