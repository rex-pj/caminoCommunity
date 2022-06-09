using Camino.Shared.Commons;
using Camino.Shared.Constants;

namespace Camino.Shared.Exceptions
{
    public class CaminoApplicationException : Exception
    {
        public string Code { get; private set; }
        public IEnumerable<CommonError> Errors { get; protected set; }
        
        public CaminoApplicationException() : base(ErrorMessages.UnexpectedException)
        {
            Code = ErrorMessages.UnexpectedException;
        }

        public CaminoApplicationException(CommonError error)
            : base(error.Message)
        {
            Code = error.Code;
        }

        public CaminoApplicationException(IEnumerable<CommonError> errors)
            : base()
        {
            Errors = errors;
        }

        public CaminoApplicationException(string message)
            : base(message)
        {
            Code = ErrorMessages.Exception;
        }

        public CaminoApplicationException(string message, string code)
            : base(message)
        {
            Code = code;
        }

        public CaminoApplicationException(Exception exception)
            : base(exception.Message, exception)
        {
            Code = ErrorMessages.Exception;
        }
    }
}
