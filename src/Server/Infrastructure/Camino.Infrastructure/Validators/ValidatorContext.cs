using Camino.Core.DependencyInjection;
using Camino.Core.Validators;

namespace Camino.Infrastructure.Validators
{
    public class ValidatorContext : BaseValidatorContext, IScopedDependency
    {
        private BaseValidator _validator;
        public override void SetValidator(BaseValidator validator)
        {
            _validator = validator;
        }

        public override TOut Validate<TIn, TOut>(TIn value)
        {
            var result = _validator.IsValid<TIn, TOut>(value);

            Errors = _validator.Errors;
            return result;
        }
    }
}
