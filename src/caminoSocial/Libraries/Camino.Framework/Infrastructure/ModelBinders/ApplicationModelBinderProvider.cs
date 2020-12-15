using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Camino.Framework.Infrastructure.ModelBinders
{
    public class ApplicationModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var dateTimeTypes = new Type[]
            {
                typeof(DateTimeOffset),
                typeof(DateTimeOffset?),
                typeof(DateTime),
                typeof(DateTime?)
            };

            if (dateTimeTypes.Contains(context.Metadata.ModelType))
            {
                return new DateTimeModelBinder();
            }

            return null;
        }
    }
}
