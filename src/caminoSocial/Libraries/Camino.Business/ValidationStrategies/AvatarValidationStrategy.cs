using System;
using System.Collections.Generic;
using System.Linq;
using Camino.Business.ValidationStrategies.Contracts;
using Camino.Core.Exceptions;
using Camino.Core.Utils;
using Camino.Business.Dtos.General;

namespace Camino.Business.ValidationStrategies
{
    public class AvatarValidationStrategy : IValidationStrategy
    {
        public IEnumerable<ErrorDto> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            var data = value as UserPhotoUpdation;

            if (string.IsNullOrEmpty(data.PhotoUrl))
            {
                Errors = GetErrors(new ArgumentNullException(data.PhotoUrl));
            }

            var image = ImageUtil.Base64ToImage(data.PhotoUrl);
            if (image.Width < 100 || image.Height < 100)
            {
                Errors = GetErrors(new PhotoSizeInvalidException(nameof(data.PhotoUrl)));
            }

            return Errors == null || !Errors.Any();
        }

        public IEnumerable<ErrorDto> GetErrors(Exception e)
        {
            yield return new ErrorDto
            {
                Message = e.Message
            };
        }
    }
}
