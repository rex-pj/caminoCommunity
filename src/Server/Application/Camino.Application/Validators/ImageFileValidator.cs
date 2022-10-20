using Camino.Core.Validators;
using Camino.Shared.Utils;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.Validators
{
    public class ImageFileValidator : BaseValidator<byte[], bool>
    {
        public override bool IsValid(byte[] value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }

                ImageUtils.FileDataToImage(value);
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
