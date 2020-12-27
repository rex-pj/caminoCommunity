using Camino.Core.Infrastructure.Extensions;
using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Infrastructure.Middlewares;
using Camino.Framework.Infrastructure.ModelBinders;
using Camino.Management.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            var mvcBuilder = services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Insert(0, new ApplicationModelBinderProvider());
            })
            .AddNewtonsoftJson();

            mvcBuilder.AddModular();
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
