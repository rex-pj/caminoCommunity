using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.MutationTypes;
using Api.Public.QueryTypes;
using Api.Public.Resolvers;
using Api.Public.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Public.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services
                .AddTransient<IUserResolver, UserResolver>()
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<UserQueryType>()
                .AddMutationType<UserMutationType>()
                .AddType<RegisterInputType>()
                .AddType<SigninInputType>()
                .AddType<RegisterInputType>()
                .AddType<SigninInputType>()
                .AddType<UserMutationType>()
                .AddType<UserQueryType>()
                .AddType(typeof(ListType<>))
                .AddType<ResetPasswordInputType>()
                .AddType<AccessModeEnumType>()
                .AddType<ApiErrorType>()
                .AddType<GenderSelectOptionType>()
                .AddType<CountrySelectOptionType>()
                .AddType<FindUserInputType>()
                .AddType<ActiveUserInputType>()
                .AddType<ForgotPasswordInputType>()
                .Create());
        }
    }
}
