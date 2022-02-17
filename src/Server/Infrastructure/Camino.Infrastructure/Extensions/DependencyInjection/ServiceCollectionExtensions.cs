using Camino.Infrastructure.Strategies.Validations;
using Microsoft.Extensions.DependencyInjection;
using Camino.Infrastructure.Linq2Db.Extensions.DependencyInjection;

namespace Camino.Infrastructure.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        internal static readonly string[] _dependencyProjectNames = new string[] {
            "Camino.Infrastructure",
            "Camino.Infrastructure.Linq2Db",
            "Camino.Core"
        };

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ValidationStrategyContext>();
            services.AddLinq2DbInfrastructureServices();
            services.AddDependencyServices(_dependencyProjectNames);
        }
    }
}
