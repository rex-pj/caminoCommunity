using Coco.Framework.Providers.Contracts;
using Coco.Framework.Providers.Implementation;
using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace Coco.Framework.Infrastructure.Middlewares
{
    public static class ModularApplicationBuilderExtensions
    {
        public static void UseModular(this IApplicationBuilder applicationBuilder)
        {
            foreach (var action in ModularManager.GetInstances<IModularConfigureAction>().OrderBy(a => a.Priority))
            {
                action.Execute(applicationBuilder, applicationBuilder.ApplicationServices);
            }
        }
    }
}
