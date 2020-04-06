using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.MutationTypes;
using Api.Identity.QueryTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.SessionManager.Contracts;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Identity.Infrastructure.Extensions
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
                .AddType<ApiErrorType>()
                .AddType<UpdatePerItemInputType>()
                .AddType<SelectOptionType>()
                .AddType<UserPasswordUpdateInputType>()
                .AddType<UserIdentifierUpdateInputType>()
                .AddType<UserPhotoUpdateInputType>()
                .AddType<DeleteUserPhotoInputType>()
                .AddType<IApiResult>()
                .Create());
        }
    }
}
