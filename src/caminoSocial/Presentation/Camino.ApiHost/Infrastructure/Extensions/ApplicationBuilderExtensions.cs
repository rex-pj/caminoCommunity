using Camino.Core.Infrastructure;
using Camino.Core.Models;
using Camino.Framework.Infrastructure.Middlewares;
using Camino.Framework.Models;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Camino.ApiHost.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var modules = Singleton<IList<ModuleInfo>>.Instance;
            var appSettings = app.ApplicationServices.GetRequiredService<IOptions<AppSettings>>().Value;
            app.UseHttpsRedirection()
                .UseRouting()
                .UseCors(appSettings.MyAllowSpecificOrigins)
                .UseAuthentication()
                .UseAuthorization()
                .UseWebSockets()
                .UseModular(env)
                .UseGraphQL("/graphql")
                .UsePlayground()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });

            return app;
        }
    }
}
