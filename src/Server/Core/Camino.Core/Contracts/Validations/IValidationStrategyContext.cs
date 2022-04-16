using Camino.Shared.Results.Errors;
using System.Collections.Generic;

namespace Camino.Core.Contracts.Validations
{
    public interface IValidationStrategyContext
    {
        IEnumerable<BaseErrorResult> Errors { get; set; }
        void SetStrategy(IValidationStrategy validationStrategy);
        bool Validate<T>(T value);
    }
}
