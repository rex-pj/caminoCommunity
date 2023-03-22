using Microsoft.AspNetCore.Builder;

namespace Camino.Management.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {
            app.UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization();

            return app;
        }
    }
}
