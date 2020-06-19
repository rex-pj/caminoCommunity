using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.DAL
{
    public static class DalStartup
    {
        public static void ConfigureContentDataAccess(this IServiceCollection services, string connectionName)
        {
            var configuration = services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>();

            services.AddLinqToDbContext<ContentDbConnection>((provider, options) => {
                options.UseSqlServer(configuration.GetConnectionString(connectionName))
                .UseDefaultLogging(provider);
            });
        }
    }
}
