using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Camino.Core.Modular.Contracts;
using Microsoft.AspNetCore.Hosting;
using Module.Web.Navigation.Infrastructure.Extensions;

namespace Module.Web.Navigation
{
    public class Startup : PluginStartupBase
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
