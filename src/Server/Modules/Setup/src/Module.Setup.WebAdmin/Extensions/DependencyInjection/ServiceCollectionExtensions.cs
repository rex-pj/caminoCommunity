using Camino.Application.Contracts;
using Camino.Core.Contracts.Providers;
using Camino.Infrastructure;
using Camino.Infrastructure.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Module.Setup.WebAdmin.Providers;

namespace Module.Setup.WebAdmin.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSetupDataAccessServices<TContext>(this IServiceCollection services) where TContext : DbContext, IAppDbContext
        {
            services.AddDbContext<IAppDbContext, TContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServerWithLazyLoading(services, "CaminoEntities");
            });
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSetupDataAccessServices<AppDbContext>();
            services.AddSingleton<SetupSettings>();
            services.AddScoped<ISetupProvider, SetupProvider>();
            return services;
        }
    }
}
