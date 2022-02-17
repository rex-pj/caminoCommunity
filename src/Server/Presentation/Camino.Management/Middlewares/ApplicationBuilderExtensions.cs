using Microsoft.AspNetCore.Builder;

namespace Camino.Management.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseManagementConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization();

            return app;
        }
    }
}
