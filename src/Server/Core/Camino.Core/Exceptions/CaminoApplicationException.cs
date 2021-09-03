using Camino.Core.Constants;
using System;
using System.Collections.Generic;
using Camino.Shared.General;

namespace Camino.Core.Exceptions
{
    public class CaminoApplicationException : Exception
    {
        public string Code { get; private set; }
        public IEnumerable<CommonError> Errors { get; protected set; }
        
        public CaminoApplicationException() : base(ErrorMessageConst.UnexpectedException)
        {
            Code = ErrorMessageConst.UnexpectedException;
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
            Code = ErrorMessageConst.Exception;
        }

        public CaminoApplicationException(string message, string code)
            : base(message)
        {
            Code = code;
        }

        public CaminoApplicationException(Exception exception)
            : base(exception.Message, exception)
        {
            Code = ErrorMessageConst.Exception;
        }
    }
}
