using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Module.Setup.WebAdmin.Extensions.DependencyInjection;
using Camino.Infrastructure.Modularity;
using Module.Setup.WebAdmin.Middlewares;
using System.Reflection;
using Camino.Infrastructure.DependencyInjection;

namespace Module.Setup.WebAdmin
{
    public class Startup : ModuleStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddModuleDependencies(Assembly.GetExecutingAssembly());
            services.ConfigureServices();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureAppBuilder();
        }
    }
}
