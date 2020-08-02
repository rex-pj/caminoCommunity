using Camino.Business.ValidationStrategies.Contracts;
using Camino.Business.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Camino.Business.Dtos.General;

namespace Camino.Business.ValidationStrategies
{
    public class UserProfileUpdateValidationStratergy : IValidationStrategy
    {
        public IEnumerable<ErrorDto> Errors { get; set; }
        public UserProfileUpdateValidationStratergy() { }

        public bool IsValid<T>(T data)
        {
            var model = data as UserIdentifierUpdateDto;
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

        public IEnumerable<ErrorDto> GetErrors(Exception e)
        {
            yield return new ErrorDto
            {
                Message = e.Message
            };
        }
    }
}
