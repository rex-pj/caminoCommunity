using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Auth.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Camino.Core.Modular.Contracts;
using Camino.Framework.Models;

namespace Module.Api.Auth
{
    public class Startup : PluginStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            services.ConfigureAuthServices(configuration);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureAppBuilder();
        }
    }
}
