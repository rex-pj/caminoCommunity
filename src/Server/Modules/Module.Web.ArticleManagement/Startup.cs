using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Module.Web.ArticleManagement.Extensions.DependencyInjection;
using Camino.Infrastructure.Modularity;
using Module.Web.ArticleManagement.Middlewares;

namespace Module.Web.ArticleManagement
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
