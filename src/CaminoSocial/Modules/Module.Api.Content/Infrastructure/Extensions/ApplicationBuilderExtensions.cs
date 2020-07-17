using Camino.Framework.Models;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Module.Api.Content.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureContentAppBuilder(this IApplicationBuilder app)
        {
            var appSettings = app.ApplicationServices.GetRequiredService<IOptions<AppSettings>>().Value;
            // Config UseCors
            app.UseHttpsRedirection()
                .UseRouting()
                .UseCors(appSettings.MyAllowSpecificOrigins)
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
