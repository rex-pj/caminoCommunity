using Camino.Framework.Infrastructure.Extensions;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Camino.Infrastructure.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Camino.Framework.GraphQL;
using System.Threading.Tasks;
using Camino.Infrastructure.Commons.Constants;

namespace Camino.ApiHost.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApiHostServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtBearerServices(services.BuildServiceProvider())
                .AddApplicationServices(configuration);

            services.AddInfrastructureServices();
            services.AddHttpContextAccessor()
                .ConfigureCorsServices(services.BuildServiceProvider());

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddModular();
            services.AddAutoMappingModular();
            return services;
        }

        public static IServiceCollection AddJwtBearerServices(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            var jwtConfigOptions = serviceProvider.GetRequiredService<IOptions<JwtConfigOptions>>().Value;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwt =>
            {
                var secretKey = Encoding.ASCII.GetBytes(jwtConfigOptions.SecretKey);
                jwt.SaveToken = true;
                jwt.RequireHttpsMetadata = false;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey), // Add the secret key to our Jwt encryption
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ValidIssuer = jwtConfigOptions.Issuer,
                    ValidAudience = jwtConfigOptions.Audience
                };
            });

            services.AddGraphQLServer()
                .AddHttpRequestInterceptor<GraphQlRequestInterceptor>()
                .AddQueryType(x => x.Name("Query"))
                .AddMutationType(x => x.Name("Mutation"))
                .AddAuthorization();

            return services;
        }

        public static IServiceCollection ConfigureCorsServices(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
            services.AddCors(options =>
            {
                options.AddPolicy(appSettings.MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(appSettings.AllowOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
