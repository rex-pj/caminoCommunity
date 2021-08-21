using Camino.Core.Contracts.FileStore;
using Camino.Infrastructure.Commons.Constants;
using Camino.Infrastructure.FileStores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.IO;

namespace Module.Web.UploadManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
            services.AddSingleton<IFileStore, Base64FileStore>();
            services.AddSingleton(typeof(IMediaFileStore), s =>
            {
                var webHostEnvironment = s.GetRequiredService<IWebHostEnvironment>();
                var appDataPath = Path.Combine(webHostEnvironment.ContentRootPath, AppDataSettings.MediaPath);
                return new LocalMediaFileStore(appDataPath);
            });

            return services;
        }
    }
}
