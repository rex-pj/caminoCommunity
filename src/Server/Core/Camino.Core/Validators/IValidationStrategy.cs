using System;
using System.Collections.Generic;

namespace Camino.Core.Validators
{
    public interface IValidationStrategy
    {
        IEnumerable<ValidatorErrorResult> Errors { get; set; }
        bool IsValid<T>(T value);
        IEnumerable<ValidatorErrorResult> GetErrors(Exception exception);
    }
}
