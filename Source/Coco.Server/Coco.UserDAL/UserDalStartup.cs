using Coco.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.IdentityDAL
{
    public class UserDalStartup : IBootstrapper
    {
        private readonly string _connectionString;

        public UserDalStartup(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("CocoUserEntities");
        }

        public void RegiserTypes(IServiceCollection services)
        {
            services.AddTransient<IConfigurationRoot, ConfigurationRoot>();
            services.AddDbContext<CocoUserDbContext>(x => x.UseSqlServer(_connectionString), ServiceLifetime.Scoped);
        }
    }
}
