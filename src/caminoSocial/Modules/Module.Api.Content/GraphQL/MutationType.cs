using Module.Api.Content.GraphQL.InputTypes;
using Module.Api.Content.GraphQL.Resolvers.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Camino.Core.Modular.Contracts;
using Module.Api.Content.GraphQL.ResultTypes;

namespace Module.Api.Content.GraphQL
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

            descriptor.Field<IArticleResolver>(x => x.CreateArticleAsync(default))
               .Type<ArticleResultType>()
               .Directive<AuthenticationDirectiveType>()
               .Argument("criterias", a => a.Type<ArticleInputType>());

            descriptor.Field<IArticleCategoryResolver>(x => x.GetCategories(default))
                .Type<ListType<SelectOptionType>>()
                .Argument("criterias", a => a.Type<SelectFilterInputType>());

            descriptor.Field<IImageResolver>(x => x.ValidateImageUrl(default))
                 .Type<CommonResultType>()
                 .Argument("criterias", a => a.Type<ImageValidationInputType>());
        }
    }
}
