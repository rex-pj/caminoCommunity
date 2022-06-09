using Camino.Core.Domains;
using Camino.Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Infrastructure.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        internal static readonly string[] _dependencyProjectNames = new string[] 
        {
            "Camino.Infrastructure",
            "Camino.Infrastructure.EntityFrameworkCore",
            "Camino.Core",
            "Camino.Application",
        };

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IAppDbContext, CaminoDbContext>();
            services.AddScoped<IDbContext, CaminoDbContext>();

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IEntityRepository<>), typeof(EfRepository<>));
            services.AddDependencyServices(_dependencyProjectNames);
        }
    }
}
