using Camino.Core.Infrastructure.Extensions;
using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Infrastructure.Middlewares;
using Camino.Management.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Camino.Management
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureManagementServices(_configuration);

            var rootPath = Directory.GetParent(_webHostEnvironment.ContentRootPath).Parent.FullName;
            var modulesPath = $"{rootPath}{_configuration["Modular:Path"]}";
            var mvcBuilder = services.AddControllersWithViews()
                .AddNewtonsoftJson();

            mvcBuilder.AddModular(modulesPath, _configuration["Modular:Prefix"]);
            services.AddAutoMappingModular();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseManagementConfiguration();
            app.UseModular(env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
