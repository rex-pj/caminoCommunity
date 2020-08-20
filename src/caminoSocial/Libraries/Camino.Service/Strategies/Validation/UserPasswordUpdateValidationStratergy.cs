using System;
using System.Collections.Generic;
using System.Linq;
using Camino.Service.Strategies.Validation.Contracts;
using Camino.Service.Data.Identity;
using Camino.Service.Data.Common;

namespace Camino.Service.Strategies.Validation
{
    public class UserPasswordUpdateValidationStratergy : IValidationStrategy
    {
        public IEnumerable<ErrorResult> Errors { get; set; }

        public IEnumerable<ErrorResult> GetErrors(Exception e)
        {
            yield return new ErrorResult
            {
                Message = e.Message
            };
        }

        public bool IsValid<T>(T value)
        {
            var data = value as UserPasswordUpdateDto;

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
