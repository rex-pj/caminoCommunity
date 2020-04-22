using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Public.MutationTypes
{
    public class UserMutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.SignupAsync(default))
                .Type<ApiResultType>()
                .Argument("criterias", a => a.Type<SignupInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().SignupAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.SigninAsync(default))
                .Type<UserTokenResultType>()
                .Argument("criterias", a => a.Type<SigninInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().SigninAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.ForgotPasswordAsync(default))
                .Type<ApiResultType>()
                .Argument("criterias", a => a.Type<NonNullType<ForgotPasswordInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ForgotPasswordAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.ResetPasswordAsync(default))
                .Type<UserTokenResultType>()
                .Argument("criterias", a => a.Type<NonNullType<ResetPasswordInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ResetPasswordAsync(ctx));
        }
    }
}
