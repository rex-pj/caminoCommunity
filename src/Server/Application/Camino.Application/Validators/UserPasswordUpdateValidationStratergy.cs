using Camino.Application.Contracts.AppServices.Authentication.Dtos;
using Camino.Core.Validators;

namespace Camino.Application.Validators
{
    public class UserPasswordUpdateValidationStratergy : IValidationStrategy
    {
        public IEnumerable<ValidatorErrorResult> Errors { get; set; }

        public IEnumerable<ValidatorErrorResult> GetErrors(Exception e)
        {
            yield return new ValidatorErrorResult
            {
                Message = e.Message
            };
        }

        public bool IsValid<T>(T value)
        {
            var data = value as UserPasswordUpdateRequest;

            if (data == null)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(data)));
            }

            if (string.IsNullOrWhiteSpace(data.CurrentPassword))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(data.CurrentPassword)));
            }

            if (string.IsNullOrWhiteSpace(data.NewPassword))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(data.NewPassword)));
            }

            return Errors == null || !Errors.Any();
        }
    }
}
