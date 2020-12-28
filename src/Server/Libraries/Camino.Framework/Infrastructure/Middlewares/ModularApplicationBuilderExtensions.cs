using Camino.Core.Models;
using Camino.Core.Modular.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Camino.Framework.Infrastructure.Middlewares
{
    public static class ModularApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseModular(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceProvider = app.ApplicationServices;
            var modules = serviceProvider.GetService(typeof(IList<ModuleInfo>)) as IList<ModuleInfo>;
            var moduleStartupInterfaceType = typeof(IModuleStartup);
            foreach (var module in modules)
            {
                var moduleStartupType = module.Assembly.GetTypes().FirstOrDefault(x => moduleStartupInterfaceType.IsAssignableFrom(x));
                if (moduleStartupType != null && moduleStartupType != moduleStartupInterfaceType)
                {
                    var moduleInitializer = Activator.CreateInstance(moduleStartupType) as IModuleStartup;
                    moduleInitializer.Configure(app, env);
                }

                var wwwrootDir = new DirectoryInfo(Path.Combine(module.Path, "wwwroot"));
                if (wwwrootDir.Exists)
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(wwwrootDir.FullName),
                        RequestPath = new PathString("/" + module.ShortName)
                    });
                }
            }

            return app;
        }
    }
}
