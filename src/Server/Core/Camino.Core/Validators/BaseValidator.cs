namespace Camino.Core.Validators
{
    public abstract class BaseValidator
    {
        public virtual IEnumerable<ValidatorErrorResult> Errors { get; protected set; }
        public abstract IEnumerable<ValidatorErrorResult> GetErrors(Exception exception);
    }

    public abstract class BaseValidator<TIn, TOut> : BaseValidator
    {
        public abstract TOut IsValid(TIn value);
    }
}
