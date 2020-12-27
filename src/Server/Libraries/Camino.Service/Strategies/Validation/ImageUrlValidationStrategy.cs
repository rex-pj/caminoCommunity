using System;
using System.Collections.Generic;
using Camino.Service.Projections.Error;
using Camino.Service.Strategies.Validation.Contracts;
using Camino.Core.Utils;

namespace Camino.Service.Strategies.Validation
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
