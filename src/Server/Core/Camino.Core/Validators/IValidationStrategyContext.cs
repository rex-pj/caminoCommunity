using System.Collections.Generic;

namespace Camino.Core.Validators
{
    public interface IValidationStrategyContext
    {
        IEnumerable<ValidatorErrorResult> Errors { get; set; }
        void SetStrategy(IValidationStrategy validationStrategy);
        bool Validate<T>(T value);
    }
}
