using Module.Api.Content.GraphQLTypes.InputTypes;
using Module.Api.Content.Resolvers.Contracts;
using Camino.Framework.GraphQLTypes.DirectiveTypes;
using Camino.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;
using Camino.Core.Modular.Contracts;

namespace Module.Api.Content.GraphQLTypes
{
    public class MutationType : BaseMutationType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IImageResolver>(x => x.ValidateImageUrl(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<ImageValidationInputType>());

            descriptor.Field<IUserPhotoResolver>(x => x.UpdateAvatarAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<UserPhotoUpdateInputType>());

            descriptor.Field<IUserPhotoResolver>(x => x.UpdateCoverAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<UserPhotoUpdateInputType>());

            descriptor.Field<IUserPhotoResolver>(x => x.DeleteAvatarAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<DeleteUserPhotoInputType>());

            descriptor.Field<IUserPhotoResolver>(x => x.DeleteCoverAsync(default))
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<DeleteUserPhotoInputType>());
        }
    }
}
