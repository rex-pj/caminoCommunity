using AutoMapper;
using Coco.Business;
using Coco.Business.Contracts;
using Coco.Business.Implementation;
using Coco.Business.MappingProfiles;
using Coco.Contract;
using Coco.Framework.Infrastructure;
using Coco.Framework.Infrastructure.MappingProfiles;
using Coco.Framework.Models;
using Coco.Management.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Coco.Management
{
    public class Startup
    {
        private readonly IBootstrapper _bootstrapper;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _bootstrapper = new BusinessStartup(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InvokeInitialStartup(services);

            services.AddAuthentication(IdentitySettings.APP_SESSION_SCHEMA)
                .AddCookie(IdentitySettings.APP_SESSION_SCHEMA);

            services.AddScoped<ISeedDataBusiness, SeedDataBusiness>();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        private void InvokeInitialStartup(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), 
                typeof(ContentMappingProfile), 
                typeof(IdentityMappingProfile), 
                typeof(AuthMappingProfile));

            FrameworkStartup.AddCustomStores(services);
            _bootstrapper.RegiserTypes(services);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (seedDataBusiness.CanSeed())
            {
                seedDataBusiness.SeedingData();
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
