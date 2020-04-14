using System;
using System.Collections.Generic;
using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Common.Utils;

namespace Coco.Business.ValidationStrategies
{
    public class Base64ImageValidationStrategy : IValidationStrategy
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

                ImageUtils.Base64ToImage(value.ToString());
                return true;
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
