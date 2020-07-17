using Camino.Management.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Camino.Management.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseManagementConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public static IApplicationBuilder UseDatabaseSettingUp(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DatabaseSettingUpMiddleware>();
        }
    }
}
