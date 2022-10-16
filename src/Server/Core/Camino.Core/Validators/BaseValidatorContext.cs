namespace Camino.Core.Validators
{
    public abstract class BaseValidatorContext
    {
        protected BaseValidator Validator { get; set; }
        public virtual IEnumerable<ValidatorErrorResult> Errors { get; protected set; }
        public abstract void SetValidator<TIn, TOut>(BaseValidator<TIn, TOut> validator);
        public abstract TOut Validate<TIn, TOut>(TIn value);
    }
}
