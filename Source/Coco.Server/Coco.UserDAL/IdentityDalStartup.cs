using Coco.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.IdentityDAL
{
    public class IdentityDalStartup : IBootstrapper
    {
        private readonly string _connectionString;

        public IdentityDalStartup(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("IdentityEntities");
        }

        public void RegiserTypes(IServiceCollection services)
        {
            services.AddTransient<IConfigurationRoot, ConfigurationRoot>()
                .AddDbContext<IdentityDbContext>
                (x => x.UseLazyLoadingProxies().UseSqlServer(_connectionString));
        }
    }
}
