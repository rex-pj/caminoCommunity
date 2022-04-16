using Microsoft.Extensions.DependencyInjection;

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
            services.AddDependencyServices(_dependencyProjectNames);
        }
    }
}
