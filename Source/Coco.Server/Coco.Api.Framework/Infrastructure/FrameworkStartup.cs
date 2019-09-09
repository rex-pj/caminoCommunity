using Coco.Api.Framework.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Coco.Api.Framework.Infrastructure
{
    public static class FrameworkStartup
    {
        public static void AddCustomStores(IServiceCollection services)
        {
            services.AddUserIdentity();
        }
    }
}
