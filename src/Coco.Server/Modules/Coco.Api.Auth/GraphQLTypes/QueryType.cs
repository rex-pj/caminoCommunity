using Coco.Api.Auth.GraphQLTypes.InputTypes;
using Coco.Api.Auth.GraphQLTypes.ResultTypes;
using Coco.Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.DirectiveTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace  Coco.Api.Auth.GraphQLTypes
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
