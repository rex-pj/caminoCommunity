using Camino.Business.Dtos.General;
using System;
using System.Collections.Generic;

namespace Camino.Business.ValidationStrategies.Contracts
{
    public interface IValidationStrategy
    {
        IEnumerable<ErrorDto> Errors { get; set; }
        bool IsValid<T>(T value);
        IEnumerable<ErrorDto> GetErrors(Exception exception);
    }
}
