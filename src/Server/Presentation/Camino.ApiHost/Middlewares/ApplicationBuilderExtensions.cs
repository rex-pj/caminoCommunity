using Camino.Shared.Configuration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Camino.ApiHost.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {
            var appSettings = app.ApplicationServices.GetRequiredService<IOptions<ApplicationSettings>>().Value;
            app.UseRouting()
               .UseCookiePolicy()
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
               });

            return app;
        }
    }
}
