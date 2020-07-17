using Module.Api.Auth.GraphQLTypes.InputTypes;
using Module.Api.Auth.GraphQLTypes.ResultTypes;
using Module.Api.Auth.Resolvers.Contracts;
using Camino.Framework.GraphQLTypes.DirectiveTypes;
using Camino.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQLTypes
{
    public class QueryType : ObjectType
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
