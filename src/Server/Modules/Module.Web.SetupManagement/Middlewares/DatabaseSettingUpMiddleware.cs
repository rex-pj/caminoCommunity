using Camino.Core.Contracts.Providers;
using Camino.Shared.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Module.Web.SetupManagement.Middlewares
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
            if (!installProvider.HasInitializedSetup())
            {
                await _next(context);
                return;
            }

            var currentUrl = context.Request.Path.ToString();
            // check the database is installed
            if (!installProvider.HasDatabaseSetup())
            {
                //redirect
                var installUrl = SetupConfigs.StartSetupUrl;
                if (!currentUrl.StartsWith(installUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Response.Redirect(installUrl);
                }

                await _next(context);
                return;
            }

            if (!installProvider.HasDataSeeded())
            {
                //redirect
                var seedDataUrl = SetupConfigs.SeedDataUrl;
                if (!currentUrl.StartsWith(seedDataUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Response.Redirect(seedDataUrl);
                }

                await _next(context);
                return;
            }

            await _next(context);
        }
    }
}
