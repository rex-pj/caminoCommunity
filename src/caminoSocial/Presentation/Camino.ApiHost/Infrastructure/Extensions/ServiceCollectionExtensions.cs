using Camino.Business;
using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Models;
using Camino.Framework.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Camino.ApiHost.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApiHostServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication();
            services.AddApplicationServices(configuration);
            services.AddBusinessServices();
            services.AddHttpContextAccessor();
            services.ConfigureCorsServices(services.BuildServiceProvider());

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
