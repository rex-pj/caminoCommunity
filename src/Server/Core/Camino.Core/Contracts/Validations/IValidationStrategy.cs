using Camino.Shared.Results.Errors;
using System;
using System.Collections.Generic;

namespace Camino.Core.Contracts.Validations
{
    public interface IValidationStrategy
    {
        IEnumerable<BaseErrorResult> Errors { get; set; }
        bool IsValid<T>(T value);
        IEnumerable<BaseErrorResult> GetErrors(Exception exception);
    }
}
