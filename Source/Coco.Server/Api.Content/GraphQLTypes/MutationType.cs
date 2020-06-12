using Api.Content.GraphQLTypes.InputTypes;
using Api.Content.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.DirectiveTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Content.GraphQLTypes
{
    public class MutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IImageResolver>(x => x.ValidateImageUrl(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<ImageValidationInputType>())
                .Resolver(ctx => ctx.Service<IImageResolver>().ValidateImageUrl(ctx));

            descriptor.Field<IUserPhotoResolver>(x => x.UpdateAvatarAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<UserPhotoUpdateInputType>())
                .Resolver(ctx => ctx.Service<IUserPhotoResolver>().UpdateAvatarAsync(ctx));

            descriptor.Field<IUserPhotoResolver>(x => x.UpdateCoverAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<UserPhotoUpdateInputType>())
                .Resolver(ctx => ctx.Service<IUserPhotoResolver>().UpdateCoverAsync(ctx));

            descriptor.Field<IUserPhotoResolver>(x => x.DeleteAvatarAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<DeleteUserPhotoInputType>())
                .Resolver(ctx => ctx.Service<IUserPhotoResolver>().DeleteAvatarAsync(ctx));

            descriptor.Field<IUserPhotoResolver>(x => x.DeleteCoverAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<DeleteUserPhotoInputType>())
                .Resolver(ctx => ctx.Service<IUserPhotoResolver>().DeleteCoverAsync(ctx));
        }
    }
}
