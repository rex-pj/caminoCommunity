using Coco.IdentityDAL.Contracts;
using Coco.IdentityDAL.Implementations;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.IdentityDAL
{
    public static class IdentityDalStartup
    {
        public static void ConfigureIdentityDataAccess(this IServiceCollection services, string connectionName)
        {
            var configuration = services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>();

            services.AddLinqToDbContext<IdentityDbConnection>((provider, options) => {
                options.UseSqlServer(configuration.GetConnectionString(connectionName))
                .UseDefaultLogging(provider);
            });

            services.AddScoped<IIdentityDataProvider, IdentityDataProvider>();
        }
    }
}
