using Api.Content.GraphQLTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Models;
using Coco.Framework.SessionManager;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Content.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            services.AddTransient<ILoginManager<ApplicationUser>, LoginManager>();
            return services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType(typeof(ListType<>))
                .AddType<AccessModeEnumType>()
                .AddType<CommonErrorType>()
                .Create());
        }
    }
}
