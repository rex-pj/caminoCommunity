using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;

namespace Module.Api.Auth.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAuthAppBuilder(this IApplicationBuilder app)
        {
            app.UseGraphQL("/graphql");
            app.UsePlayground("/graphql", "/playground");
            return app;
        }
    }
}
