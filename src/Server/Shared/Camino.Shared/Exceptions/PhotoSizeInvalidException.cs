using Camino.Shared.Constants;

namespace Camino.Shared.Exceptions
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

        public PhotoSizeInvalidException(string message)
            : base(message)
        {
            Code = ErrorMessages.PhotoSizeInvalidException;
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="minWidth">The minimum with of the photo (in pixel)</param>
        /// <param name="minHeight">The minimum height of the photo (in pixel)</param>
        public PhotoSizeInvalidException(int minWidth, int minHeight) : base($"Photo should larger than {minWidth}px X {minHeight}px")
        {
            Code = ErrorMessages.PhotoSizeInvalidException;
        }
    }
}
