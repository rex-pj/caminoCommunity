using Camino.Core.Validators;
using Camino.Infrastructure.AspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Auth.Api.Validators
{
    public class PartialUpdateValidator : BaseValidator<PartialUpdateModel, bool>
    {
        public override bool IsValid(PartialUpdateModel value)
        {
            if (string.IsNullOrEmpty(value.Key))
            {
                Errors = GetErrors(new ArgumentException(nameof(value.Updates))).ToList();
            }

            if (value.Updates == null || !value.Updates.Any())
            {
                Errors = GetErrors(new ArgumentException(nameof(value.Updates))).ToList();
            }

            foreach (var updateItem in value.Updates)
            {
                if (updateItem.PropertyName == null)
                {
                    Errors = GetErrors(new ArgumentException(nameof(updateItem.PropertyName))).ToList();
                }
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
