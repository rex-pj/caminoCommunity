using Camino.Core.Contracts.Providers;
using Microsoft.Extensions.DependencyInjection;
using Module.Setup.WebAdmin.Providers;

namespace Module.Setup.WebAdmin.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<SetupSettings>();
            services.AddScoped<ISetupProvider, SetupProvider>();
            return services;
        }
    }
}
