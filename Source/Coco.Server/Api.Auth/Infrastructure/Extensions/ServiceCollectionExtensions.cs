using Api.Auth.GraphQLTypes;
using Api.Auth.GraphQLTypes.InputTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Models;
using Coco.Framework.SessionManager;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Auth.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddTransient<ILoginManager<ApplicationUser>, LoginManager>();
            return services
                .AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddType<CommonErrorType>()
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
                .AddType<ICommonResult>()
                .Create());
        }
    }
}
