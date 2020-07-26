using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Module.Api.Auth.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Camino.Core.Modular.Contracts;

namespace Module.Api.Auth
{
    public class Startup : PluginStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            services.ConfigureAuthServices(configuration);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Config UseCors
            app.ConfigureAuthAppBuilder();
        }
    }
}
