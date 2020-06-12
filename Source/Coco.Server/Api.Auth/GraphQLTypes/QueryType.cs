using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.DirectiveTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.SignoutAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Resolver(ctx => ctx.Service<IUserResolver>().SignoutAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.GetLoggedUser(default))
                .Type<FullUserInfoResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Resolver(ctx => ctx.Service<IUserResolver>().GetLoggedUser(ctx));

            descriptor.Field<IUserPhotoResolver>(x => x.GetUserAvatar(default))
                .Type<UserAvatarResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>())
                .Resolver(ctx => ctx.Service<IUserPhotoResolver>().GetUserAvatar(ctx));

            descriptor.Field<IUserPhotoResolver>(x => x.GetUserPhotos(default))
                .Type<ListType<UserPhotoResultType>>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>())
                .Resolver(ctx => ctx.Service<IUserPhotoResolver>().GetUserPhotos(ctx));

            descriptor.Field<IUserPhotoResolver>(x => x.GetUserCover(default))
                .Type<UserCoverResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>())
                .Resolver(ctx => ctx.Service<IUserPhotoResolver>().GetUserCover(ctx));

            descriptor.Field<IUserResolver>(x => x.GetFullUserInfoAsync(default))
                .Type<FullUserInfoResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().GetFullUserInfoAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.ActiveAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<ActiveUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ActiveAsync(ctx));
        }
    }
}
