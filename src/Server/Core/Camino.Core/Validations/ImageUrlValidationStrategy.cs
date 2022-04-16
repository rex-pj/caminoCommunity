using System;
using System.Collections.Generic;
using Camino.Shared.Results.Errors;
using Camino.Core.Contracts.Validations;
using Camino.Core.Utils;

namespace Camino.Core.Validations
{
    public class ImageUrlValidationStrategy : IValidationStrategy
    {
        public IEnumerable<BaseErrorResult> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }

                return ImageUtil.IsImageUrl(value.ToString());
            }
            catch (Exception e)
            {
                Errors = GetErrors(e);
                return false;
            }
        }

        public IEnumerable<BaseErrorResult> GetErrors(Exception exception)
        {
            yield return new BaseErrorResult() {
                Message = exception.Message
            };
        }
    }
}
