using Camino.Core.Validators;
using Camino.Shared.Utils;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Camino.Shared.File;
using System.Threading.Tasks;

namespace Camino.Application.Validators
{
    public class ImageFormFileValidator : BaseAsyncValidator<IFormFile, bool>
    {
        public override async Task<bool> IsValidAsync(IFormFile value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }

                var fileData = await FileUtils.GetBytesAsync(value);
                ImageUtils.FileDataToImage(fileData);
                return true;
            }
            catch (Exception e)
            {
                Errors = GetErrors(e).ToList();
                return false;
            }
        }

        public override IEnumerable<ValidatorErrorResult> GetErrors(Exception exception)
        {
            yield return new ValidatorErrorResult() {
                Message = exception.Message
            };
        }
    }
}
