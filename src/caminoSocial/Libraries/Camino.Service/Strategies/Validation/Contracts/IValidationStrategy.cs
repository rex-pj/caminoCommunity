using Camino.Service.Data.Common;
using System;
using System.Collections.Generic;

namespace Camino.Service.Strategies.Validation.Contracts
{
    public interface IValidationStrategy
    {
        IEnumerable<ErrorResult> Errors { get; set; }
        bool IsValid<T>(T value);
        IEnumerable<ErrorResult> GetErrors(Exception exception);
    }
}
