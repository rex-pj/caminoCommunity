using Microsoft.AspNetCore.Builder;

namespace Module.Api.Navigation.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
