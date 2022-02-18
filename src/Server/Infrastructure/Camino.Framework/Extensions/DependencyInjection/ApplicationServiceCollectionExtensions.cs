﻿using Camino.Framework.Helpers;
using Camino.Shared.Configurations;
using Camino.IdentityManager.Contracts.Core;
using Camino.Core.Domain.Identities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.Core.Contracts.Helpers;
using Camino.IdentityManager.Extensions.DependencyInjection;

namespace Camino.Framework.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.Name));
            services.Configure<CrypterSettings>(configuration.GetSection(CrypterSettings.Name));
            services.Configure<EmailSenderSettings>(configuration.GetSection(EmailSenderSettings.Name));
            services.Configure<PagerOptions>(configuration.GetSection(PagerOptions.Name));
            services.Configure<JwtConfigOptions>(configuration.GetSection(JwtConfigOptions.Name));

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>()
                .AddScoped<IHttpHelper, HttpHelper>()
                .AddScoped<IJwtHelper, JwtHelper>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}