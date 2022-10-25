using Microsoft.Extensions.DependencyInjection;

namespace Module.Article.WebAdmin.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
