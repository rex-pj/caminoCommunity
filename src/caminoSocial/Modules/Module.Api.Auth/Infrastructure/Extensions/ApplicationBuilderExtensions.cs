using Microsoft.AspNetCore.Builder;

namespace Module.Api.Auth.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAuthAppBuilder(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
