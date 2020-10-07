using Module.Api.Media.GraphQL.InputTypes;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Camino.Core.Modular.Contracts;

namespace Module.Api.Media.GraphQL
{
    public class MutationType : BaseMutationType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
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

            descriptor.Field<IImageResolver>(x => x.ValidateImageUrl(default))
                 .Type<CommonResultType>()
                 .Argument("criterias", a => a.Type<ImageValidationInputType>());
        }
    }
}
