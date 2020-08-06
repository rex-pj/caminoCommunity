using Camino.Framework.Infrastructure.Middlewares;
using Camino.Framework.Models;
using Camino.Framework.Models.Settings;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Camino.ApiHost.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var appSettings = app.ApplicationServices.GetRequiredService<IOptions<AppSettings>>().Value;
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(appSettings.MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseModular(env);

            app.UseWebSockets();
            app.UseGraphQL("/graphql");
            app.UsePlayground();

            return app;
        }
    }
}
