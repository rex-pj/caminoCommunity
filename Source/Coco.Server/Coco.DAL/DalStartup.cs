using Coco.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.DAL
{
    public class DalStartup : IBootstrapper
    {
        private readonly string _connectionString;

        public DalStartup(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("CocoEntities");
        }

        public void RegiserTypes(IServiceCollection services)
        {
            services.AddDbContext<CocoDbContext>
                (x => x.UseSqlServer(_connectionString));
        }
    }
}
