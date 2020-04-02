using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.Resolvers.Contracts;
using HotChocolate.Types;

namespace Api.Public.MutationTypes
{
    public class UserMutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("signup")
                .Argument("user", a => a.Type<NonNullType<RegisterInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().SignupAsync(ctx));

            descriptor.Field("signin")
                .Argument("args", a => a.Type<NonNullType<SigninInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().SigninAsync(ctx));

            descriptor.Field("forgotPassword")
                .Argument("criterias", a => a.Type<NonNullType<ForgotPasswordInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ForgotPasswordAsync(ctx));

            descriptor.Field("resetPassword")
                .Argument("criterias", a => a.Type<NonNullType<ResetPasswordInputType>>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ResetPasswordAsync(ctx));
        }
    }
}
