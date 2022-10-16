using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Core.Validators;

namespace Camino.Application.Validators
{
    public class UserProfileValidator : BaseValidator<UserIdentifierUpdateRequest, bool>
    {
        public override bool IsValid(UserIdentifierUpdateRequest data)
        {
            if (data == null)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(data))).ToList();
            }

            if (data.Id <= 0)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(data.Id))).ToList();
            }

            if (string.IsNullOrWhiteSpace(data.Lastname))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(data.Lastname))).ToList();
            }

            if (string.IsNullOrWhiteSpace(data.Firstname))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(data.Firstname))).ToList();
            }

            if (string.IsNullOrWhiteSpace(data.DisplayName))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(data.DisplayName))).ToList();
            }

            return Errors == null || !Errors.Any();
        }

        public override IEnumerable<ValidatorErrorResult> GetErrors(Exception e)
        {
            yield return new ValidatorErrorResult
            {
                Message = e.Message
            };
        }
    }
}
