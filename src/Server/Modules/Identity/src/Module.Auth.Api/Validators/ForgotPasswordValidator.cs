using Camino.Core.Validators;
using Module.Auth.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Auth.Api.Validators
{
    public class ForgotPasswordValidator : BaseValidator<ForgotPasswordModel, bool>
    {
        public override bool IsValid(ForgotPasswordModel value)
        {
            if (value == null)
            {
                Errors = GetErrors(new ArgumentException(nameof(value))).ToList();
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
