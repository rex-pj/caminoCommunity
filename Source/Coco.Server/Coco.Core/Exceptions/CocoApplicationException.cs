using Coco.Core.Constants;
using Coco.Core.Dtos.General;
using System;
using System.Collections.Generic;

namespace Coco.Core.Exceptions
{
    public class CocoApplicationException : Exception
    {
        public string Code { get; private set; }
        public IEnumerable<CommonError> Errors { get; protected set; }
        
        public CocoApplicationException() : base(ErrorMessageConst.UN_EXPECTED_EXCEPTION)
        {
            Code = ErrorMessageConst.UN_EXPECTED_EXCEPTION;
        }

        public CocoApplicationException(CommonError error)
            : base(error.Message)
        {
            Code = error.Code;
        }

        public CocoApplicationException(IEnumerable<CommonError> errors)
            : base()
        {
            Errors = errors;
        }

        public CocoApplicationException(string message)
            : base(message)
        {
            Code = ErrorMessageConst.EXCEPTION;
        }

        public CocoApplicationException(string message, string code)
            : base(message)
        {
            Code = code;
        }

        public CocoApplicationException(Exception exception)
            : base(exception.Message, exception)
        {
            Code = ErrorMessageConst.EXCEPTION;
        }
    }
}
