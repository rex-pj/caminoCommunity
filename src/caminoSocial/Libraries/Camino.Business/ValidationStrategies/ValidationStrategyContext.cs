using Camino.Business.Dtos.General;
using Camino.Business.ValidationStrategies.Contracts;
using System.Collections.Generic;

namespace Camino.Business.ValidationStrategies
{
    public class ValidationStrategyContext
    {
        private IValidationStrategy _validationStrategy;
        public IEnumerable<ErrorDto> Errors { get; set; }

        public ValidationStrategyContext()
        {
            Errors = new List<ErrorDto>();
        }

        public ValidationStrategyContext(IValidationStrategy validationStrategy)
        {
            Errors = new List<ErrorDto>();
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
