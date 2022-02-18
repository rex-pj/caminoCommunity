﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.IdentityManager.Contracts.Core;
using Camino.Shared.Configurations;
using Camino.Framework.Extensions.DependencyInjection;
using Camino.Infrastructure.Extensions.DependencyInjection;

namespace Camino.Management.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices(configuration);
            services.AddAuthentication(IdentitySettings.APP_SESSION_SCHEMA).AddCookie(IdentitySettings.APP_SESSION_SCHEMA);
            services.AddInfrastructureServices();
            services.AddDataProtection();

            services.AddSingleton<SetupSettings>();

            return services;
        }
    }
}