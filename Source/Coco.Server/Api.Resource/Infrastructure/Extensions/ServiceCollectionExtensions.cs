using Api.Resource.GraphQLTypes;
using Api.Resource.GraphQLTypes.InputTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Models;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Resource.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType<AccessModeEnumType>()
                .AddType<ApiErrorType>()
                .AddType<SelectOptionType>()
                .AddType<ImageValidationInputType>()
                .AddType<IApiResult>()
                .Create());
        }
    }
}
