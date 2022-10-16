using Camino.Core.Validators;

namespace Camino.Application.Validators
{
    public class PhoneValidator : BaseValidator<object, bool>
    {
        private const string AdditionalPhoneNumberCharacters = "-.()";
        private const string ExtensionAbbreviationExtDot = "ext.";
        private const string ExtensionAbbreviationExt = "ext";
        private const string ExtensionAbbreviationX = "x";

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is not string valueAsString)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(valueAsString)));
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
                Errors = GetErrors(new ArgumentException($"Non numberic of {nameof(digitFound)}"));
                return false;
            }

            foreach (char c in valueAsString)
            {
                if (!(char.IsDigit(c)
                    || char.IsWhiteSpace(c)
                    || AdditionalPhoneNumberCharacters.IndexOf(c) != -1))
                {
                    Errors = GetErrors(new ArgumentException($"Non numberic of {nameof(valueAsString)}"));
                    return false;
                }
            }

            return true;
        }

        private string RemoveExtension(string potentialPhoneNumber)
        {
            var lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(ExtensionAbbreviationExtDot, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + ExtensionAbbreviationExtDot.Length);
                if (MatchesExtension(extension))
                {
                    return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
                }
            }

            lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(ExtensionAbbreviationExt, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + ExtensionAbbreviationExt.Length);
                if (MatchesExtension(extension))
                {
                    return potentialPhoneNumber.Substring(0, lastIndexOfExtension);
                }
            }

            lastIndexOfExtension = potentialPhoneNumber
                .LastIndexOf(ExtensionAbbreviationX, StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfExtension >= 0)
            {
                var extension = potentialPhoneNumber.Substring(
                    lastIndexOfExtension + ExtensionAbbreviationX.Length);
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
