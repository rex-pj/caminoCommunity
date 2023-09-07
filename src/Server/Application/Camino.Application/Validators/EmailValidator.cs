using Camino.Core.Validators;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace Camino.Application.Validators
{
    public class EmailValidator : BaseValidator<object, bool>
    {
        private const string _pattern = "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is not string valueAsString)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(valueAsString))).ToList();
                return false;
            }

            return Regex.IsMatch(valueAsString, _pattern);
        }

        public override IEnumerable<ValidatorErrorResult> GetErrors(Exception exception)
        {
            yield return new ValidatorErrorResult()
            {
                Message = exception.Message
            };
        }
    }
}
