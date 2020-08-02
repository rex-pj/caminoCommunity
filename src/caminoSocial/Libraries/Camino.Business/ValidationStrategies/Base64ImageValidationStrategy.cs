using System;
using System.Collections.Generic;
using Camino.Business.Dtos.General;
using Camino.Business.ValidationStrategies.Contracts;
using Camino.Core.Utils;

namespace Camino.Business.ValidationStrategies
{
    public class Base64ImageValidationStrategy : IValidationStrategy
    {
        public IEnumerable<ErrorDto> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }

                ImageUtil.Base64ToImage(value.ToString());
                return true;
            }
            catch (Exception e)
            {
                Errors = GetErrors(e);
                return false;
            }
        }

        public IEnumerable<ErrorDto> GetErrors(Exception exception)
        {
            yield return new ErrorDto() {
                Message = exception.Message
            };
        }
    }
}
