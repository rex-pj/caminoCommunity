using Camino.Service.Data.Common;
using Camino.Service.Strategies.Validation.Contracts;
using System.Collections.Generic;

namespace Camino.Service.Strategies.Validation
{
    public class ValidationStrategyContext
    {
        private IValidationStrategy _validationStrategy;
        public IEnumerable<ErrorResult> Errors { get; set; }

        public ValidationStrategyContext()
        {
            Errors = new List<ErrorResult>();
        }

        public ValidationStrategyContext(IValidationStrategy validationStrategy)
        {
            Errors = new List<ErrorResult>();
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
