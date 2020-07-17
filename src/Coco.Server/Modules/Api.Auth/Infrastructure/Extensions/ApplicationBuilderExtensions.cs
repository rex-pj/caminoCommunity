using Coco.Framework.Models;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.Auth.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureContentAppBuilder(this IApplicationBuilder app)
        {
            var cocoSettings = app.ApplicationServices.GetRequiredService<IOptions<CocoSettings>>().Value;
            // Config UseCors
            app.UseHttpsRedirection()
                .UseRouting()
                .UseCors(cocoSettings.MyAllowSpecificOrigins)
                .UseAuthentication()
                .UseAuthorization()
                .UseWebSockets()
                .UseGraphQL("/api/graphql")
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            return app;
        }
    }
}
