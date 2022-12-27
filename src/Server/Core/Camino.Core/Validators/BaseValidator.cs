namespace Camino.Core.Validators
{
    public abstract class BaseValidator
    {
        public virtual IList<ValidatorErrorResult> Errors { get; protected set; }
        public abstract IEnumerable<ValidatorErrorResult> GetErrors(Exception exception);
    }

    public abstract class BaseValidator<TIn, TOut> : BaseValidator
    {
        public abstract TOut IsValid(TIn value);
    }

    public abstract class BaseAsyncValidator<TIn, TOut> : BaseValidator
    {
        public abstract Task<TOut> IsValidAsync(TIn value);
    }
}
