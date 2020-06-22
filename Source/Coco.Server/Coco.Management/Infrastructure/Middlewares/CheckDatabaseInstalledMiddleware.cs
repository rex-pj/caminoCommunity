using Coco.Framework.Providers.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Coco.Management.Infrastructure.Middlewares
{
    public class CheckDatabaseInstalledMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckDatabaseInstalledMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IInstallProvider installProvider)
        {
            // check the database is installed
            if (!installProvider.IsDatabaseInstalled && installProvider.IsInitialized)
            {
                //redirect
                var installUrl = installProvider.LoadSettings().InstallUrl;
                var currentUrl = context.Request.Path.ToString();
                if (!currentUrl.StartsWith(installUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Response.Redirect(installUrl);
                    return;
                }
            }

            await _next(context);
        }
    }
}
