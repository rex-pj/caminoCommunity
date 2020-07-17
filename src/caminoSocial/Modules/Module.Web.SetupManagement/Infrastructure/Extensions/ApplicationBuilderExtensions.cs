using Microsoft.AspNetCore.Builder;
using Module.Web.SetupManagement.Infrastructure.Middlewares;

namespace Module.Web.SetupManagement.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {
            app.UseDatabaseSettingUp();
            return app;
        }

        public static IApplicationBuilder UseDatabaseSettingUp(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DatabaseSettingUpMiddleware>();
        }
    }
}
