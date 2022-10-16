using Camino.Core.Validators;
using Camino.Shared.Utils;

namespace Camino.Application.Validators
{
    public class ImageUrlValidator : BaseValidator<string, bool>
    {
        public override bool IsValid(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                return ImageUtils.IsImageUrl(value.ToString());
            }
            catch (Exception e)
            {
                Errors = GetErrors(e);
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
