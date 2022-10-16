﻿using Camino.Application.Contracts.AppServices.Authentication.Dtos;
using Camino.Core.Validators;

namespace Camino.Application.Validators
{
    public class UserPasswordUpdateValidator : BaseValidator<UserPasswordUpdateRequest, bool>
    {
        public override IEnumerable<ValidatorErrorResult> GetErrors(Exception e)
        {
            yield return new ValidatorErrorResult
            {
                Message = e.Message
            };
        }

        public override bool IsValid(UserPasswordUpdateRequest value)
        {
            if (value == null)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(value)));
            }

            if (string.IsNullOrWhiteSpace(value.CurrentPassword))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(value.CurrentPassword)));
            }

            if (string.IsNullOrWhiteSpace(value.NewPassword))
            {
                Errors = GetErrors(new ArgumentNullException(nameof(value.NewPassword)));
            }

            return Errors == null || !Errors.Any();
        }
    }
}
