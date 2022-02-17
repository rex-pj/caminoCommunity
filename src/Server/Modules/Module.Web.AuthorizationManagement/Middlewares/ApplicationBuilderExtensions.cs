using Microsoft.AspNetCore.Builder;

namespace Module.Web.AuthorizationManagement.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
