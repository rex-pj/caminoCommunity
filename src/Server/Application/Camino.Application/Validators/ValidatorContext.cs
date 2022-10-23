using Camino.Core.DependencyInjection;
using Camino.Core.Validators;

namespace Camino.Application.Validators
{
    public class ValidatorContext : BaseValidatorContext, IScopedDependency
    {
        public override void SetValidator<TIn, TOut>(BaseValidator<TIn, TOut> validator)
        {
            Validator = validator;
        }

        public override TOut Validate<TIn, TOut>(TIn value)
        {
            var validator = Validator as BaseValidator<TIn, TOut>;
            var result = validator.IsValid(value);

            Errors = Validator.Errors;
            return result;
        }
    }
}
