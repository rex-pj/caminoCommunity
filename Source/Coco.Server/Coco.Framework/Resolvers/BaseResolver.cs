using Coco.Entities.Models;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Framework.Resolvers
{
    public abstract class BaseResolver
    {
        protected virtual void HandleContextError(IResolverContext context, IEnumerable<CommonError> errors)
        {
            if (errors != null && errors.Any())
            {
                foreach (var error in errors)
                {
                    context.ReportError(error.Message);
                }
            }
        }

        protected virtual void HandleContextError(IResolverContext context, IEnumerable<IdentityError> errors)
        {
            if (errors != null && errors.Any())
            {
                foreach (var error in errors)
                {
                    context.ReportError(error.Description);
                }
            }
        }

        protected virtual void HandleContextError(IResolverContext context, Exception error)
        {
            context.ReportError(error.Message);
        }
    }
}
