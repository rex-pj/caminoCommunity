using Api.Public.GraphQLSchemas;
using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.Mutations;
using Api.Public.Queries;
using Api.Public.Resolvers;
using Api.Public.Resolvers.Contracts;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Public.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlDependency(this IServiceCollection services)
        {
            return services.AddSingleton<IDocumentExecuter, DocumentExecuter>()
                .AddSingleton<IDocumentWriter, DocumentWriter>()
                .AddTransient<IUserResolver, UserResolver>()
                .AddSingleton<RegisterInputType>()
                .AddSingleton<SigninInputType>()
                .AddTransient<UserMutation>()
                .AddTransient<UserQuery>()
                .AddSingleton(typeof(ListGraphType<>))
                .AddSingleton<ResetPasswordInputType>()
                .AddSingleton<AccessModeEnumType>()
                .AddSingleton<ApiErrorType>()
                .AddSingleton<GenderSelectOptionType>()
                .AddSingleton<CountrySelectOptionType>()
                .AddSingleton<FindUserInputType>()
                .AddSingleton<ActiveUserInputType>()
                .AddSingleton<ForgotPasswordInputType>()
                .AddSingleton<ApiResultType>()
                .AddSingleton<RegisterResultType>()
                .AddSingleton<UserInfoResultType>()
                .AddSingleton<UserTokenResultType>()
                .AddSingleton<FullUserInfoResultType>()
                .AddSingleton<ActiveUserResultType>()
                .AddSingleton<ResetPasswordResultType>()
                .AddSingleton<ApiResultType<UserInfoExtend, FullUserInfoResultType>, ApiFullUserInfoResultType>()
                .AddSingleton<ApiResultType<UserTokenResult, UserTokenResultType>, ApiUserTokenResultType>()
                .AddSingleton<LoggedInUserResultType>()
                .AddScoped<ISchema, UserSchema>();
        }
    }
}
