using Camino.Core.Contracts.Providers;
using Microsoft.Extensions.DependencyInjection;
using Module.Web.SetupManagement.Providers;

namespace Module.Web.SetupManagement.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            services.AddScoped<ISetupProvider, SetupProvider>();
            return services;
        }
    }
}
