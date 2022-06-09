using System;
using Camino.Shared.Constants;

namespace Camino.Infrastructure.Images.Exceptions
{
    public class PhotoSizeInvalidException : Exception
    {
        public string Code { get; private set; }
        /// <summary>
        /// Initializes a new instance of the Exception class.
        /// </summary>
        public PhotoSizeInvalidException()
        {
            Code = ErrorMessages.PhotoSizeInvalidException;
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PhotoSizeInvalidException(string message)
            : base(message)
        {
            Code = ErrorMessages.PhotoSizeInvalidException;
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
