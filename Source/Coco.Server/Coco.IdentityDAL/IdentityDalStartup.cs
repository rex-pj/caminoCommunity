using Microsoft.EntityFrameworkCore;
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

            var connectionString = configuration.GetConnectionString(connectionName);
            services.AddDbContext<IdentityDbContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
