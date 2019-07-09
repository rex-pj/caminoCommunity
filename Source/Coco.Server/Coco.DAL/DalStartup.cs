using Coco.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.DAL
{
    public class DalStartup : IBootstrapper
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public DalStartup(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("CocoEntities");
        }

        public void RegiserTypes(IServiceCollection services)
        {
            services.AddTransient<IConfigurationRoot, ConfigurationRoot>()
                .AddDbContext<CocoDbContext>
                (x => x.UseSqlServer(_connectionString));
        }
    }
}
