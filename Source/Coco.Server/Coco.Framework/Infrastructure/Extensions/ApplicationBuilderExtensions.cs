using Microsoft.AspNetCore.Builder;

namespace Coco.Framework.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBasicApiMiddleware(this IApplicationBuilder app)
        {
            return app
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
