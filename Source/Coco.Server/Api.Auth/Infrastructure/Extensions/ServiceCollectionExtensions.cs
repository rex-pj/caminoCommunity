using Api.Auth.GraphQLTypes;
using Api.Auth.GraphQLTypes.InputTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Models;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Auth.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            services.AddAuthentication();

            return services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType<ApiErrorType>()
                .AddType<UpdatePerItemInputType>()
                .AddType<SelectOptionType>()
                .AddType<FindUserInputType>()
                .AddType<UserPasswordUpdateInputType>()
                .AddType<UserIdentifierUpdateInputType>()
                .AddType<UserPhotoUpdateInputType>()
                .AddType<DeleteUserPhotoInputType>()
                .AddType<SignupInputType>()
                .AddType<ResetPasswordInputType>()
                .AddType<AccessModeEnumType>()
                .AddType<ActiveUserInputType>()
                .AddType<ForgotPasswordInputType>()
                .AddType<IApiResult>()
                .Create());
        }
    }
}
