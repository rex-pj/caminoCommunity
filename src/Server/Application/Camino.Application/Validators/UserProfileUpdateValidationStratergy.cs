using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Core.Validators;

namespace Camino.Application.Validators
{
    public class UserProfileUpdateValidationStratergy : IValidationStrategy
    {
        public IEnumerable<ValidatorErrorResult> Errors { get; set; }
        public UserProfileUpdateValidationStratergy() { }

        public bool IsValid<T>(T data)
        {
            var model = data as UserIdentifierUpdateRequest;
            if (model == null)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model)));
            }

            if (model.Id <= 0)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model.Id)));
            }

            if (string.IsNullOrWhiteSpace(model.Lastname))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model.Lastname)));
            }

            if (string.IsNullOrWhiteSpace(model.Firstname))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model.Firstname)));
            }

            if (string.IsNullOrWhiteSpace(model.DisplayName))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(model.DisplayName)));
            }

            return Errors == null || !Errors.Any();
        }

        public IEnumerable<ValidatorErrorResult> GetErrors(Exception e)
        {
            yield return new ValidatorErrorResult
            {
                Message = e.Message
            };
        }
    }
}
