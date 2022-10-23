using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Camino.Infrastructure.AspNetCore.ModelBinders
{
    public class ApplicationModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var dateTimeTypes = new Type[]
            {
                typeof(DateTime),
                typeof(DateTime?),
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
