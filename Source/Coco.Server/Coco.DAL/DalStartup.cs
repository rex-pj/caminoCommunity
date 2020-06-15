using Microsoft.EntityFrameworkCore;
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

            var connectionString = configuration.GetConnectionString(connectionName);
            services.AddDbContext<ContentDbContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
