using Camino.Core.Validators;
using Camino.Shared.Utils;

namespace Camino.Application.Validators
{
    public class Base64ImageValidator : BaseValidator<string, bool>
    {
        public override bool IsValid(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                ImageUtils.Base64ToImage(value.ToString());
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
