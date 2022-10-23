using Camino.Application.Contracts;
using Camino.Core.Contracts.Providers;
using Camino.Infrastructure;
using Camino.Infrastructure.EntityFrameworkCore.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Module.Web.SetupManagement.Providers;

namespace Module.Web.SetupManagement.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSetupDataAccessServices<TContext>(this IServiceCollection services) where TContext : DbContext, IAppDbContext
        {
            services.AddDbContextPool<IAppDbContext, TContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServerWithLazyLoading(services, "CaminoEntities");
            });
        }

        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            services.AddSetupDataAccessServices<AppDbContext>();
            services.AddSingleton<SetupSettings>();
            services.AddScoped<ISetupProvider, SetupProvider>();
            return services;
        }
    }
}
