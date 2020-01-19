using Coco.Api.Framework.GraphQLTypes.Redefines;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Content.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDocumentExecuter, DocumentExecuter>()
                .AddSingleton(typeof(ListGraphType<>))
                .AddSingleton<AccessModeEnumType>()
                .AddSingleton<ApiErrorType>()
                .AddSingleton<DynamicGraphType>()
                .AddSingleton<ApiResultType<UserTokenResult, UserTokenResultType>, ApiUserTokenResultType>()
                .AddSingleton<SignoutResultType>()
                .AddSingleton<UserTokenResultType>()
                .AddSingleton<UserInfoResultType>();
        }
    }
}
