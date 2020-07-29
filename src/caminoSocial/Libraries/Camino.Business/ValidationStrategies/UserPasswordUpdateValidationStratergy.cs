using System;
using System.Collections.Generic;
using System.Linq;
using Camino.Business.ValidationStrategies.Interfaces;
using Camino.Business.ValidationStrategies.Models;
using Camino.Business.Dtos.Identity;

namespace Camino.Business.ValidationStrategies
{
    public class UserPasswordUpdateValidationStratergy : IValidationStrategy
    {
        public IEnumerable<ErrorObject> Errors { get; set; }

        public IEnumerable<ErrorObject> GetErrors(Exception e)
        {
            yield return new ErrorObject
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
