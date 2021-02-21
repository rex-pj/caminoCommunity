using Camino.Shared.Results.Errors;
using Camino.Core.Contracts.Strategies.Validations;
using System.Collections.Generic;

namespace Camino.Infrastructure.Strategies.Validations
{
    public class ValidationStrategyContext
    {
        private IValidationStrategy _validationStrategy;
        public IEnumerable<BaseErrorResult> Errors { get; set; }

        public ValidationStrategyContext()
        {
            Errors = new List<BaseErrorResult>();
        }

        public ValidationStrategyContext(IValidationStrategy validationStrategy)
        {
            Errors = new List<BaseErrorResult>();
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
