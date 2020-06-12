﻿using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.DirectiveTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes
{

    public class MutationType : ObjectType
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

            // Public mutation
            descriptor.Field<IUserResolver>(x => x.SignupAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<SignupInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().SignupAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.SigninAsync(default))
                .Type<UserTokenResultType>()
                .Argument("criterias", a => a.Type<SigninInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().SigninAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.ForgotPasswordAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<NonNullType<ForgotPasswordInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ForgotPasswordAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.ResetPasswordAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<NonNullType<ResetPasswordInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ResetPasswordAsync(ctx));
        }
    }
}
