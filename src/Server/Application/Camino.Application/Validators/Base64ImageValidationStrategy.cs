using Camino.Core.Validators;
using Camino.Shared.Utils;

namespace Camino.Application.Validators
{
    public class Base64ImageValidationStrategy : IValidationStrategy
    {
        public IEnumerable<ValidatorErrorResult> Errors { get; set; }

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

        public IEnumerable<ValidatorErrorResult> GetErrors(Exception exception)
        {
            yield return new ValidatorErrorResult() {
                Message = exception.Message
            };
        }
    }
}
