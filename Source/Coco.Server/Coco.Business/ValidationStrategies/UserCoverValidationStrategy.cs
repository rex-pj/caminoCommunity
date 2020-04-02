using System;
using System.Collections.Generic;
using System.Linq;
using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Common.Exceptions;
using Coco.Common.Utils;
using Coco.Entities.Dtos.General;

namespace Coco.Business.ValidationStrategies
{
    public class UserCoverValidationStrategy : IValidationStrategy
    {
        public IEnumerable<ErrorObject> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            var data = value as UserPhotoUpdateDto;

            if (string.IsNullOrEmpty(data.PhotoUrl))
            {
                Errors = GetErrors(new PhotoSizeInvalidException(nameof(data.PhotoUrl)));
            }

            var image = ImageUtils.Base64ToImage(data.PhotoUrl);
            if (image.Width < 1000 || image.Height < 300)
            {
                Errors = GetErrors(new PhotoSizeInvalidException(nameof(data.PhotoUrl)));
            }

            return Errors == null || !Errors.Any();
        }

        public IEnumerable<ErrorObject> GetErrors(Exception e)
        {
            yield return new ErrorObject
            {
                Message = e.Message
            };
        }
    }
}
