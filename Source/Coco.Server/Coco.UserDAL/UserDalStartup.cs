using Coco.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.UserDAL
{
    public class UserDalStartup : IBootstrapper
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public UserDalStartup(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("CocoUserEntities");
        }

        public void RegiserTypes(IServiceCollection services)
        {
            services.AddTransient<IConfigurationRoot, ConfigurationRoot>();
            services.AddDbContext<CocoUserDbContext>(x => x.UseSqlServer(_connectionString), ServiceLifetime.Scoped);
        }
    }
}
