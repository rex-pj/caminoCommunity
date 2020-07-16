using System;
using System.Collections.Generic;
using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Core.Utils;

namespace Coco.Business.ValidationStrategies
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
