using Api.Content.GraphQLTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Content.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType(typeof(ListType<>))
                .AddType<AccessModeEnumType>()
                .AddType<ApiErrorType>()
                .Create());
        }
    }
}
