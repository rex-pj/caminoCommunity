using Camino.Core.Constants;
using System;
using System.Collections.Generic;
using Camino.Shared.General;

namespace Camino.Core.Exceptions
{
    public class CaminoAuthenticationException : Exception
    {
        public string Code { get; private set; }
        public IEnumerable<CommonError> Errors { get; protected set; }
        
        public CaminoAuthenticationException() : base(ErrorMessageConst.UnexpectedException)
        {
            Code = ErrorMessageConst.UnexpectedException;
        }

        public CaminoAuthenticationException(string message)
            : base(message)
        {
            Code = ErrorMessageConst.Exception;
        }

        public CaminoAuthenticationException(string message, string code)
            : base(message)
        {
            Code = code;
        }
    }
}
