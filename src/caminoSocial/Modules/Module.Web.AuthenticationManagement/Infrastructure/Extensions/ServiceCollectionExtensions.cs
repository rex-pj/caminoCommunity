using Microsoft.Extensions.DependencyInjection;

namespace Module.Web.AuthenticationManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
