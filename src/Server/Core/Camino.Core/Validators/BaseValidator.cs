namespace Camino.Core.Validators
{
    public abstract class BaseValidator
    {
        public IEnumerable<ValidatorErrorResult> Errors { get; protected set; }
        public virtual TOut IsValid<TIn, TOut>(TIn value)
        {
            return default;
        }

        public abstract IEnumerable<ValidatorErrorResult> GetErrors(Exception exception);
    }

    public abstract class BaseValidator<TIn, TOut> : BaseValidator
    {
        public virtual TOut IsValid(TIn value)
        {
            return default;
        }
    }
}
