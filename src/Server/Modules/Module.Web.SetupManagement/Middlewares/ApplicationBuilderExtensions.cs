using Microsoft.AspNetCore.Builder;

namespace Module.Web.SetupManagement.Middlewares
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
