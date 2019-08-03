using Coco.Common.Const;
using System;
using System.Runtime.Serialization;

namespace Coco.Common.Exceptions
{
    public class PhotoSizeInvalidException: Exception
    {
        public string Code { get; private set; }
        /// <summary>
        /// Initializes a new instance of the Exception class.
        /// </summary>
        public PhotoSizeInvalidException()
        {
            Code = ErrorMessageConst.PhotoSizeInvalidException;
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PhotoSizeInvalidException(string message)
            : base(message)
        {
            Code = ErrorMessageConst.PhotoSizeInvalidException;
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="code">The code of the error.</param>
        public PhotoSizeInvalidException(string message, string code)
            : base(message)
        {
            Code = code;
        }
    }
}
