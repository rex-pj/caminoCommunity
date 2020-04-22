using Api.Resources.GraphQLTypes.InputTypes;
using Api.Resources.MutationTypes;
using Api.Resources.QueryTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Models;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Resources.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<ImageQueryType>()
                .AddMutationType<ImageMutationType>()
                .AddType<AccessModeEnumType>()
                .AddType<ApiErrorType>()
                .AddType<SelectOptionType>()
                .AddType<ImageValidationInputType>()
                .AddType<IApiResult>()
                .Create());
        }
    }
}
