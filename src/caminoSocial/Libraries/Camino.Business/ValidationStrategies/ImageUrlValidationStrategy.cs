using System;
using System.Collections.Generic;
using Camino.Business.ValidationStrategies.Interfaces;
using Camino.Business.ValidationStrategies.Models;
using Camino.Core.Utils;

namespace Camino.Business.ValidationStrategies
{
    public class ImageUrlValidationStrategy : IValidationStrategy
    {
        public IEnumerable<ErrorObject> Errors { get; set; }

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

        public IEnumerable<ErrorObject> GetErrors(Exception exception)
        {
            yield return new ErrorObject() {
                Message = exception.Message
            };
        }
    }
}
