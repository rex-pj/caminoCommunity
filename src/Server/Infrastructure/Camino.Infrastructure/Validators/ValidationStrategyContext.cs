using Camino.Core.DependencyInjection;
using Camino.Core.Validators;
using System.Collections.Generic;

namespace Camino.Infrastructure.Validators
{
    public class ValidationStrategyContext : IValidationStrategyContext, IScopedDependency
    {
        private IValidationStrategy _validationStrategy;
        public IEnumerable<ValidatorErrorResult> Errors { get; set; }

        public ValidationStrategyContext()
        {
            Errors = new List<ValidatorErrorResult>();
        }

        public ValidationStrategyContext(IValidationStrategy validationStrategy)
        {
            Errors = new List<ValidatorErrorResult>();
            _validationStrategy = validationStrategy;
        }

        public void SetStrategy(IValidationStrategy validationStrategy)
        {
            _validationStrategy = validationStrategy;
        }

        public bool Validate<T>(T value)
        {
            var result = _validationStrategy.IsValid(value);

            Errors = _validationStrategy.Errors;
            return result;
        }
    }
}
