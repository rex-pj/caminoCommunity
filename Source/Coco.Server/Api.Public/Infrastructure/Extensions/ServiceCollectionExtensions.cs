using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.MutationTypes;
using Api.Public.QueryTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Models;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Public.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<UserQueryType>()
                .AddMutationType<UserMutationType>()
                .AddType<SignupInputType>()
                .AddType<ResetPasswordInputType>()
                .AddType<AccessModeEnumType>()
                .AddType<ApiErrorType>()
                .AddType<SelectOptionType>()
                .AddType<ActiveUserInputType>()
                .AddType<ForgotPasswordInputType>()
                .AddType<IApiResult>()
                .Create());
        }
    }
}
