using AutoMapper;
using Coco.Business;
using Coco.Business.MappingProfiles;
using Coco.Contract;
using Coco.Framework.Infrastructure;
using Coco.Framework.MappingProfiles;
using Coco.Framework.Models;
using Coco.Framework.SessionManager;
using Coco.Framework.SessionManager.Contracts;
using Coco.Management.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Coco.Management
{
    public class Startup
    {
        private IBootstrapper _bootstrapper;
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

            services.AddAuthentication(IdentityConstants.ApplicationScheme).AddCookie(IdentityConstants.ApplicationScheme);
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        private void InvokeInitialStartup(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), 
                typeof(ArticleCategoryMappingProfile), 
                typeof(UserMappingProfile), 
                typeof(RoleMappingProfile),
                typeof(AuthMappingProfile));

            services.AddScoped<ISessionClaimsPrincipalFactory<ApplicationUser>, SessionClaimsPrincipalFactory<ApplicationUser>>();
            FrameworkStartup.AddCustomStores(services);
            _bootstrapper.RegiserTypes(services);

            services.AddTransient<ILoginManager<ApplicationUser>, SessionLoginManager>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
