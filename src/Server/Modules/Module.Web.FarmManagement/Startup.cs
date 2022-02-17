using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Module.Web.FarmManagement.Extensions.DependencyInjection;
using Camino.Infrastructure.Modularity;
using Module.Web.FarmManagement.Middlewares;

namespace Module.Web.FarmManagement
{
    public class Startup : ModuleStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureServices();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureAppBuilder();
        }
    }
}
