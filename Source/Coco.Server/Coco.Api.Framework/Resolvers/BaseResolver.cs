using Coco.Api.Framework.Models;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Api.Framework.Resolvers
{
    public abstract class BaseResolver
    {
        protected virtual void HandleContextError(ResolveFieldContext<object> context, IEnumerable<ApiError> errors)
        {
            if (errors != null && errors.Any() && !context.Errors.Any())
            {
                foreach (var error in errors)
                {
                    if (!context.Errors.Any(x => error.Code.Equals(x.Code)))
                    {
                        context.Errors.Add(new ExecutionError(error.Description)
                        {
                            Code = error.Code
                        });
                    }
                }
            }
        }
    }
}
