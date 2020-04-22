using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.MutationTypes;
using Api.Identity.QueryTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Middlewares;
using Coco.Framework.Models;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Identity.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            services.AddAuthentication();

            return services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<UserQueryType>()
                .AddMutationType<UserMutationType>()
                .AddType<ApiErrorType>()
                .AddType<UpdatePerItemInputType>()
                .AddType<SelectOptionType>()
                .AddType<FindUserInputType>()
                .AddType<UserPasswordUpdateInputType>()
                .AddType<UserIdentifierUpdateInputType>()
                .AddType<UserPhotoUpdateInputType>()
                .AddType<DeleteUserPhotoInputType>()
                .AddType<IApiResult>()
                .Create());
        }
    }
}
