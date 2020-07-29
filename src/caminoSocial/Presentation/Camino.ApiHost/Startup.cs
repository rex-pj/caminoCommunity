using Camino.ApiHost.Infrastructure.Extensions;
using Camino.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Camino.ApiHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApiHostServices(Configuration);

            var rootPath = Directory.GetParent(_webHostEnvironment.ContentRootPath).Parent.FullName;
            var modulesPath = $"{rootPath}{Configuration["Modular:Path"]}";
            services.AddModular(modulesPath, Configuration["Modular:Prefix"]);
            services.AddGraphQlModular();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureAppBuilder(env);
        }
    }
}
