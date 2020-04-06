using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.MutationTypes;
using Api.Public.QueryTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.SessionManager.Contracts;
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
                .Use(next => context =>
                {
                    context.ContextData["SessionContext"] = context.Service<ISessionContext>();
                    return next.Invoke(context);
                })
                .AddServices(sp)
                .AddQueryType<UserQueryType>()
                .AddMutationType<UserMutationType>()
                .AddType<SignupInputType>()
                .AddType<ResetPasswordInputType>()
                .AddType<AccessModeEnumType>()
                .AddType<ApiErrorType>()
                .AddType<SelectOptionType>()
                .AddType<FindUserInputType>()
                .AddType<ActiveUserInputType>()
                .AddType<ForgotPasswordInputType>()
                .AddType<IApiResult>()
                .Create());
        }
    }
}
