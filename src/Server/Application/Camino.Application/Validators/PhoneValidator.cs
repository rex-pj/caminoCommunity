using Camino.Core.Validators;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.Validators
{
    public class PhoneValidator : BaseValidator<object, bool>
    {
        private const string _additionalPhoneNumberCharacters = "-.()";
        private const string _extensionAbbreviationExtDot = "ext.";
        private const string _extensionAbbreviationExt = "ext";
        private const string _extensionAbbreviationX = "x";

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

            valueAsString = valueAsString.Replace("+", string.Empty).TrimEnd();
            valueAsString = RemoveExtension(valueAsString);

            bool digitFound = false;
            foreach (char c in valueAsString)
            {
                if (char.IsDigit(c))
                {
                    digitFound = true;
                    break;
                }
            }

            if (!digitFound)
            {
                Errors = GetErrors(new ArgumentException($"Non numberic of {nameof(digitFound)}")).ToList();
                return false;
            }

            foreach (char c in valueAsString)
            {
                if (!(char.IsDigit(c)
                    || char.IsWhiteSpace(c)
                    || _additionalPhoneNumberCharacters.IndexOf(c) != -1))
                {
                    Errors = GetErrors(new ArgumentException($"Non numberic of {nameof(valueAsString)}")).ToList();
                    return false;
                }
            }

            return true;
        }

        private string RemoveExtension(string potentialPhoneNumber)
        {
            var lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(_extensionAbbreviationExtDot, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + _extensionAbbreviationExtDot.Length);
                if (MatchesExtension(extension))
                {
                    return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
                }
            }

            lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(_extensionAbbreviationExt, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + _extensionAbbreviationExt.Length);
                if (MatchesExtension(extension))
                {
                    return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
                }
            }

            lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(_extensionAbbreviationX, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + _extensionAbbreviationX.Length);
                if (MatchesExtension(extension))
                {
                    return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
                }
            }

            return potentialPhoneNumber;
        }

        private bool MatchesExtension(string potentialExtension)
        {
            potentialExtension = potentialExtension.TrimStart();
            if (potentialExtension.Length == 0)
            {
                return false;
            }

            foreach (char c in potentialExtension)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
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
