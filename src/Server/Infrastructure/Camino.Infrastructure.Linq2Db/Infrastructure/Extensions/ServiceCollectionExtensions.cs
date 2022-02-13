using Camino.Core.Contracts.Data;
using LinqToDB.AspNet;
using LinqToDB.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LinqToDB;

namespace Camino.Infrastructure.Linq2Db.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLinq2DbInfrastructureServices(this IServiceCollection services)
        {
            services.AddDataAccessServices();
            services.AddScoped(typeof(IRepository<>), typeof(Linq2DbRepository<>));
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
