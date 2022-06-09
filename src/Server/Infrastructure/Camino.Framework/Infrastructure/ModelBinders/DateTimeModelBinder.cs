using Camino.Shared.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Camino.Framework.Infrastructure.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            if (!DateTimeOffset.TryParseExact(value, DateTimeFormats.ParseableFormats, 
                CultureInfo.InvariantCulture.DateTimeFormat,
                DateTimeStyles.AllowWhiteSpaces, 
                out var dateTime))
            {
                bindingContext.ModelState.TryAddModelError(modelName, $"Unable to parse {value} to datetime");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(dateTime);
            return Task.CompletedTask;
        }
    }
}
