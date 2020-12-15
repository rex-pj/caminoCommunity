using Microsoft.Extensions.DependencyInjection;

namespace Module.Web.IdentityManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
