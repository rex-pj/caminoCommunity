using Camino.Core.Contracts.Strategies.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using Camino.Shared.Results.Errors;
using Camino.Shared.Requests.Authentication;

namespace Camino.Infrastructure.Strategies.Validations
{
    public class UserProfileUpdateValidationStratergy : IValidationStrategy
    {
        public IEnumerable<BaseErrorResult> Errors { get; set; }
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

        public IEnumerable<BaseErrorResult> GetErrors(Exception e)
        {
            yield return new BaseErrorResult
            {
                Message = e.Message
            };
        }
    }
}
