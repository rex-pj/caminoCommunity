using Camino.Core.Infrastructure;
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
            var modules = Singleton<IList<ModuleInfo>>.Instance;
            var pluginStartupInterfaceType = typeof(IPluginStartup);
            foreach (var module in modules)
            {
                var pluginStartupType = module.Assembly.GetTypes().FirstOrDefault(x => pluginStartupInterfaceType.IsAssignableFrom(x));
                if (pluginStartupType != null && pluginStartupType != pluginStartupInterfaceType)
                {
                    var moduleInitializer = Activator.CreateInstance(pluginStartupType) as IPluginStartup;
                    moduleInitializer.Configure(app, env);
                }

                var wwwrootDir = new DirectoryInfo(Path.Combine(module.Path, "wwwroot"));
                if (!wwwrootDir.Exists)
                {
                    continue;
                }

                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(wwwrootDir.FullName),
                    RequestPath = new PathString("/" + module.ShortName)
                });
            }

            return app;
        }
    }
}
