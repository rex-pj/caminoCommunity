﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Module.Authorization.WebAdmin.Extensions.DependencyInjection;
using Camino.Infrastructure.Modularity;
using Module.Authorization.WebAdmin.Middlewares;

namespace Module.Authorization.WebAdmin
{
    public class Startup : ModuleStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureFileServices();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureAppBuilder();
        }
    }
}
