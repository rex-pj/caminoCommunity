using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Api.Identity.Resolvers.Contracts;
using Coco.Api.Framework.Attributes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Middlewares;
using HotChocolate.Types;

namespace Api.Identity.MutationTypes
{
    
    public class UserMutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.UpdateUserInfoItemAsync(default))
                .Type<ItemUpdatedResultType>()
                .Argument("criterias", a => a.Type<UpdatePerItemInputType>())
                .Directive<AuthenticationDirectiveType>()
                .Resolver(ctx => ctx.Service<IUserResolver>().UpdateUserInfoItemAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.UpdateIdentifierAsync(default))
                .Type<UserIdentifierUpdateResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<UserIdentifierUpdateInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().UpdateIdentifierAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.UpdatePasswordAsync(default))
                .Type<UserTokenResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<NonNullType<UserPasswordUpdateInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().UpdatePasswordAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.UpdateAvatarAsync(default))
                .Type<ApiResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<UserPhotoUpdateInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().UpdateAvatarAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.UpdateCoverAsync(default))
                .Type<ApiResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<UserPhotoUpdateInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().UpdateCoverAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.DeleteAvatarAsync(default))
                .Type<ApiResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<DeleteUserPhotoInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().DeleteAvatarAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.DeleteCoverAsync(default))
                .Type<ApiResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<DeleteUserPhotoInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().DeleteCoverAsync(ctx));
        }
    }
}
