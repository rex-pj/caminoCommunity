using Coco.Api.Framework.Models;
using HotChocolate.Resolvers;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Api.Framework.Resolvers
{
    public abstract class BaseResolver
    {
        protected virtual void HandleContextError(IResolverContext context, IEnumerable<ApiError> errors)
        {
            if (errors != null && errors.Any())
            {
                foreach (var error in errors)
                {
                    context.ReportError(error.Description);
                }
            }
        }
    }
}
