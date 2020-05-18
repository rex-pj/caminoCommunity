using Coco.Common.Const;
using Coco.Common.Models;
using System;
using System.Collections.Generic;

namespace Coco.Common.Exceptions
{
    public class CocoApplicationException : Exception
    {
        public string Code { get; private set; }
        public List<CommonError> Errors { get; protected set; }
        
        public CocoApplicationException() : base(ErrorMessageConst.UN_EXPECTED_EXCEPTION)
        {
            Code = ErrorMessageConst.UN_EXPECTED_EXCEPTION;
        }

        public CocoApplicationException(CommonError error)
            : base(error.Message)
        {
            Code = error.Code;
        }

        public CocoApplicationException(List<CommonError> errors)
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
