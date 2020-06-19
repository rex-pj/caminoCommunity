using Coco.Business.Contracts;
using Coco.Business.Implementation;
using Coco.Framework.Providers.Contracts;
using Coco.Management.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Coco.Management
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureManagementServices(_configuration);
            services.AddScoped<ISeedDataBusiness, SeedDataBusiness>();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedDataBusiness seedDataBusiness)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.ConfigureManagementAppBuilder();

            var installationProvider = app.ApplicationServices.GetRequiredService<IInstallProvider>();
            if (!installationProvider.IsDatabaseInstalled && installationProvider.IsInitialized && seedDataBusiness.CanSeed())
            {
                seedDataBusiness.SeedingData();
                installationProvider.SetDatabaseInstalled();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
