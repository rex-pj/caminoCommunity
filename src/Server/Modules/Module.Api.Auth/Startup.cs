using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Auth.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Camino.Infrastructure.Modularity;
using Module.Api.Auth.Middlewares;

namespace Module.Api.Auth
{
    public class Startup : ModuleStartupBase
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
