using Api.Auth.Models;
using Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class FullUserInfoResultType : ObjectType<FullUserInfoModel>
    {
        protected override void Configure(IObjectTypeDescriptor<FullUserInfoModel> descriptor)
        {
            descriptor.Field(x => x.Lastname).Type<StringType>();
            descriptor.Field(x => x.Firstname).Type<StringType>();
            descriptor.Field(x => x.Email).Type<StringType>();
            descriptor.Field(x => x.DisplayName).Type<StringType>();
            descriptor.Field(x => x.IsActived).Type<BooleanType>();
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
            descriptor.Field(x => x.CanEdit).Type<BooleanType>();
            descriptor.Field(x => x.GenderSelections)
                .Resolver(ctx => ctx.Service<IGenderResolver>().GetSelections())
                .Type<ListType<SelectOptionType>>();
            descriptor.Field(x => x.CountrySelections)
                .Type<ListType<CountryResultType>>()
                .Resolver(ctx => ctx.Service<ICountryResolver>().GetAll());
            descriptor.Field(x => x.AvatarUrl)
                .Resolver(async ctx => await ctx.Service<IUserPhotoResolver>().GetAvatarUrlByUserIdAsync(ctx))
                .Type<StringType>();
            descriptor.Field(x => x.CoverPhotoUrl)
                .Resolver(async ctx => await ctx.Service<IUserPhotoResolver>().GetCoverUrlByUserIdAsync(ctx))
                .Type<StringType>();
        }
    }
}
