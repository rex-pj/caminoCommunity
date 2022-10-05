namespace Camino.Core.Validators
{
    public abstract class BaseValidatorContext
    {
        public IEnumerable<ValidatorErrorResult> Errors { get; protected set; }
        public abstract void SetValidator(BaseValidator validator);
        public abstract TOut Validate<TIn, TOut>(TIn value);
    }
}
