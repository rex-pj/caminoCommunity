using Camino.ApiHost.Infrastructure.Extensions;
using Camino.Core.Models;
using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Providers.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;

namespace Camino.ApiHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IList<ModuleInfo> _modules;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var rootPath = Directory.GetParent(_webHostEnvironment.ContentRootPath).Parent.FullName;
            var modulesPath = $"{rootPath}{Configuration["Extensions:Path"]}";
            _modules = new ModularManager().LoadModules(modulesPath);
            
            services.ConfigureApiHostServices(Configuration);
            services.AddModular(_modules);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureAppBuilder(env, _modules);
        }
    }
}
