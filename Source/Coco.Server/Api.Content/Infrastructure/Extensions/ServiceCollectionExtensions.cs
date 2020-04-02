using Coco.Api.Framework.GraphQLTypes.Redefines;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
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
                .AddType(typeof(ListType<>))
                .AddType<AccessModeEnumType>()
                .AddType<ApiErrorType>()
                .AddType<DynamicType>()
                .Create());
        }
    }
}
