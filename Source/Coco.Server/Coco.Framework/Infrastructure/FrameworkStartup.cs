using Coco.Framework.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.Framework.Infrastructure
{
    public static class FrameworkStartup
    {
        public static void AddCustomStores(IServiceCollection services)
        {
            services.AddControllers();
            services.AddUserIdentity();
        }
    }
}
