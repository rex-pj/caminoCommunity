using Microsoft.AspNetCore.Builder;

namespace Module.Product.WebAdmin.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
