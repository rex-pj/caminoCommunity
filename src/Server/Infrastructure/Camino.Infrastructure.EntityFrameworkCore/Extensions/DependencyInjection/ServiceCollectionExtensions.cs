using Camino.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccessServices<TContext>(this IServiceCollection services) where TContext : DbContext, IDbContext
        {
            services.AddEntityFrameworkNpgsql();
            services.AddDbContextPool<IDbContext, TContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsqlWithLazyLoading(services, "CaminoEntities");
            });
        }

        public static void UseNpgsqlWithLazyLoading(this DbContextOptionsBuilder optionsBuilder, IServiceCollection services, string connectionKey)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(connectionKey),
                x => x.MigrationsAssembly("Camino.Infrastructure.EntityFrameworkCore.Migrations"));
        }
    }
}
