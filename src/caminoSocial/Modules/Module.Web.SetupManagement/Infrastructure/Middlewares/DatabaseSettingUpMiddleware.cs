using Camino.Framework.Providers.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Module.Web.SetupManagement.Infrastructure.Middlewares
{
    public class DatabaseSettingUpMiddleware
    {
        private readonly RequestDelegate _next;

        public DatabaseSettingUpMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ISetupProvider installProvider)
        {
            // check the database is installed
            if (!installProvider.HasSetupDatabase && installProvider.IsInitialized)
            {
                //redirect
                var installUrl = installProvider.LoadSettings().SetupUrl;
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
