using Microsoft.Extensions.DependencyInjection;

namespace Module.Web.FarmManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
