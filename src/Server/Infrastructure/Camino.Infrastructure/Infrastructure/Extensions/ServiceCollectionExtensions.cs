using Camino.Core.Contracts.Data;
using Camino.Infrastructure.Data;
using Camino.Infrastructure.Strategies.Validations;
using LinqToDB.AspNet;
using LinqToDB.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LinqToDB;
using Camino.Infrastructure.Providers;
using Camino.Core.Contracts.Providers;

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
            services.AddTransient<ValidationStrategyContext>();
            services.AddRepositoryServices();
            services.AddProviders();
            services.AddScopedServices(_dependencyProjectNames, _dependencyInterfaceNamespaces);
        }

        public static void AddProviders(this IServiceCollection services)
        {
            services.AddSingleton<IFileProvider, FileProvider>()
                .AddScoped<IEmailProvider, EmailProvider>();
        }

        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddDataAccessServices();
            services.AddTransient(typeof(IRepository<>), typeof(CaminoRepository<>));
        }

        public static void AddDataAccessServices(this IServiceCollection services)
        {
            services.AddDbConnectionServices<CaminoDataConnection>("CaminoEntities");
        }

        public static void AddDbConnectionServices<TContext>(this IServiceCollection services, string connectionKey) where TContext : IDataContext
        {
            var configuration = services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>();

            services.AddLinqToDbContext<TContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionKey));
            });
        }
    }
}
