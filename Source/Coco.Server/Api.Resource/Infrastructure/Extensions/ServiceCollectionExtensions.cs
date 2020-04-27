using Api.Resource.GraphQLTypes;
using Api.Resource.GraphQLTypes.InputTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Models;
using Coco.Framework.SessionManager;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Resource.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            services.AddTransient<ILoginManager<ApplicationUser>, LoginManager>();
            return services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType<AccessModeEnumType>()
                .AddType<CommonErrorType>()
                .AddType<SelectOptionType>()
                .AddType<ImageValidationInputType>()
                .AddType<ICommonResult>()
                .Create());
        }
    }
}
