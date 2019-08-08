using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using System.Collections.Generic;

namespace Coco.Business.ValidationStrategies
{
    public class ValidationStrategyContext
    {
        private IValidationStrategy _validationStrategy;
        public IEnumerable<ErrorObject> Errors { get; set; }

        public ValidationStrategyContext()
        {
            Errors = new List<ErrorObject>();
        }

        // Usually, the Context accepts a strategy through the constructor, but
        // also provides a setter to change it at runtime.
        public ValidationStrategyContext(IValidationStrategy validationStrategy)
        {
            Errors = new List<ErrorObject>();
            _validationStrategy = validationStrategy;
        }

        // Usually, the Context allows replacing a Strategy object at runtime.
        public void SetStrategy(IValidationStrategy validationStrategy)
        {
            _validationStrategy = validationStrategy;
        }

        // The Context delegates some work to the Strategy object instead of
        // implementing multiple versions of the algorithm on its own.
        public bool Validate<T>(T value)
        {
            var result = _validationStrategy.IsValid(value);

            Errors = _validationStrategy.Errors;
            return result;
        }
    }
}
