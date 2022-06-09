using Camino.Shared.Commons;
using Camino.Shared.Constants;

namespace Camino.Shared.Exceptions
{
    public class CaminoAuthenticationException : Exception
    {
        public string Code { get; private set; }
        public IEnumerable<CommonError> Errors { get; protected set; }
        
        public CaminoAuthenticationException() : base(ErrorMessages.UnexpectedException)
        {
            Code = ErrorMessages.UnexpectedException;
        }

        public CaminoAuthenticationException(string message)
            : base(message)
        {
            Code = ErrorMessages.Exception;
        }

        public CaminoAuthenticationException(string message, string code)
            : base(message)
        {
            Code = code;
        }
    }
}
