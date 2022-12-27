using Camino.Core.DependencyInjection;
using Camino.Core.Validators;
using System.Threading.Tasks;

namespace Camino.Application.Validators
{
    public class ValidatorContext : BaseValidatorContext, IScopedDependency
    {
        public override void SetValidator<TIn, TOut>(BaseValidator<TIn, TOut> validator)
        {
            Validator = validator;
        }

        public override void SetValidator<TIn, TOut>(BaseAsyncValidator<TIn, TOut> validator)
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

        public override async Task<TOut> ValidateAsync<TIn, TOut>(TIn value)
        {
            var validator = Validator as BaseAsyncValidator<TIn, TOut>;
            var result = await validator.IsValidAsync(value);

            Errors = Validator.Errors;
            return result;
        }
    }
}
