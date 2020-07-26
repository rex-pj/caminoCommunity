using Microsoft.AspNetCore.Builder;

namespace Module.Api.Content.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureContentAppBuilder(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
