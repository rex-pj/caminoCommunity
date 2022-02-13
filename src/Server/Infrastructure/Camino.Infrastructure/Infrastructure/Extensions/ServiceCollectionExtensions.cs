using Camino.Infrastructure.Strategies.Validations;
using Microsoft.Extensions.DependencyInjection;
using Camino.Infrastructure.Providers;
using Camino.Core.Contracts.Providers;
using Camino.Infrastructure.Linq2Db.Extensions;

namespace Camino.Infrastructure.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static readonly string[] _dependencyProjectNames = new string[] {
            "Camino.Infrastructure",
            "Camino.Core"
        };

        internal static readonly string[] _dependencyInterfaceNamespaces = new string[]
        {
            "Camino.Core.Contracts.Services",
            "Camino.Core.Contracts.Repositories",
        };

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ValidationStrategyContext>();
            services.AddLinq2DbInfrastructureServices();
            services.AddProviders();
            services.AddScopedServices(_dependencyProjectNames, _dependencyInterfaceNamespaces);
        }

        public static void AddProviders(this IServiceCollection services)
        {
            services.AddSingleton<IFileProvider, FileProvider>()
                .AddScoped<IEmailProvider, EmailProvider>();
        }
    }
}
