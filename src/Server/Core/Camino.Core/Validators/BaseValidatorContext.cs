namespace Camino.Core.Validators
{
    public abstract class BaseValidatorContext
    {
        protected BaseValidator Validator { get; set; }
        public virtual IList<ValidatorErrorResult> Errors { get; protected set; }
        public abstract void SetValidator<TIn, TOut>(BaseValidator<TIn, TOut> validator);
        public abstract void SetValidator<TIn, TOut>(BaseAsyncValidator<TIn, TOut> validator);
        public abstract TOut Validate<TIn, TOut>(TIn value);
        public abstract Task<TOut> ValidateAsync<TIn, TOut>(TIn value);
    }
}
