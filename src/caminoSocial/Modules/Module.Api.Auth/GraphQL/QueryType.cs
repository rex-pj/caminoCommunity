using Module.Api.Auth.GraphQL.InputTypes;
using Module.Api.Auth.GraphQL.ResultTypes;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Camino.Core.Modular.Contracts;

namespace  Module.Api.Auth.GraphQL
{
    public class QueryType : BaseQueryType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.SignoutAsync())
                .Type<CommonResultType>()
                .Directive<AuthenticationDirectiveType>();

            descriptor.Field<IUserResolver>(x => x.GetLoggedUser())
                .Type<FullUserInfoResultType>()
                .Directive<InitializeSessionDirectiveType>();

            descriptor.Field<IUserPhotoResolver>(x => x.GetUserAvatar(default))
                .Type<UserAvatarResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>());

            descriptor.Field<IUserPhotoResolver>(x => x.GetUserPhotos(default))
                .Type<ListType<UserPhotoResultType>>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>());

            descriptor.Field<IUserPhotoResolver>(x => x.GetUserCover(default))
                .Type<UserCoverResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>());

            descriptor.Field<IUserResolver>(x => x.GetFullUserInfoAsync(default))
                .Type<FullUserInfoResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>());

            descriptor.Field<IUserResolver>(x => x.ActiveAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<ActiveUserInputType>());
        }
    }
}
