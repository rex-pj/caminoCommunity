using Camino.Framework.Infrastructure.Middlewares;
using Camino.Shared.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Camino.ApiHost.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var appSettings = app.ApplicationServices.GetRequiredService<IOptions<AppSettings>>().Value;
            app.UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseWebSockets()
                .UseCors(appSettings.MyAllowSpecificOrigins)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapGraphQL();
                    endpoints.MapGet("/", context =>
                    {
                        context.Response.Redirect("/graphql");
                        return Task.CompletedTask;
                    });
                })
                .UseModular(env);

            return app;
        }
    }
}
