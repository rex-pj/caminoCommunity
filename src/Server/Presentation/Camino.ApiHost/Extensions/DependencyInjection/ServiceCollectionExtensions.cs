using Camino.Shared.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Camino.Framework.GraphQL;
using Microsoft.AspNetCore.Http;
using Camino.Framework.Extensions.DependencyInjection;
using Camino.Infrastructure.Extensions.DependencyInjection;

namespace Camino.ApiHost.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApiHostServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtBearerServices(services.BuildServiceProvider());

            services.AddHttpContextAccessor()
                .AddApplicationServices(configuration)
                .ConfigureCorsServices(services.BuildServiceProvider());

            services.AddInfrastructureServices();

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddModular();
            services.AddAutoMappingModular();

            return services;
        }

        public static IServiceCollection AddJwtBearerServices(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            var jwtConfigOptions = serviceProvider.GetRequiredService<IOptions<JwtConfigOptions>>().Value;
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
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
            })
#if !DEBUG
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });
#else
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        });
#endif
            services.AddGraphQLServer()
                .AddAuthorization()
                .AddHttpRequestInterceptor<GraphQlRequestInterceptor>()
                .AddQueryType(x => x.Name("Query"))
                .AddMutationType(x => x.Name("Mutation"));

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
                    builder
                        .WithOrigins(appSettings.AllowOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
