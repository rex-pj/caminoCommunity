using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.Infrastructure.Identity.Options;
using Camino.Infrastructure.DependencyInjection;
using Camino.Infrastructure.AspNetCore.ModelBinders;
using Camino.Infrastructure.Extensions.DependencyInjection;
using Camino.Infrastructure.AutoMapper.DependencyInjection;
using Camino.Infrastructure.EntityFrameworkCore.Extensions.DependencyInjection;
using Camino.Infrastructure.EntityFrameworkCore;

namespace Camino.Management.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices(configuration);
            services.AddAuthentication(IdentitySettings.APP_SESSION_SCHEMA).AddCookie(IdentitySettings.APP_SESSION_SCHEMA);
            services.AddInfrastructureServices();
            services.AddDataAccessServices<CaminoDbContext>();
            services.AddDataProtection();
            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Insert(0, new ApplicationModelBinderProvider());
            })
            .AddNewtonsoftJson()
            .AddModularManager()
            .AddModules(configuration);

            services.AutoRegisterAutoMapper();

            return services;
        }
    }
}
