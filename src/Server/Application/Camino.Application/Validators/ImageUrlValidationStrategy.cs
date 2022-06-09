using Camino.Core.Validators;
using Camino.Infrastructure.Images.Utils;

namespace Camino.Application.Validators
{
    public class ImageUrlValidationStrategy : IValidationStrategy
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

                return ImageUtils.IsImageUrl(value.ToString());
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
