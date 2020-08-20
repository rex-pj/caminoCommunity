using Camino.Service.Data.Error;
using System;
using System.Collections.Generic;

namespace Camino.Service.Strategies.Validation.Contracts
{
    public interface IValidationStrategy
    {
        IEnumerable<BaseErrorResult> Errors { get; set; }
        bool IsValid<T>(T value);
        IEnumerable<BaseErrorResult> GetErrors(Exception exception);
    }
}
