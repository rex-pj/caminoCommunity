using System.Collections.Generic;
using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Common.Utils;
using Coco.Entities.Model.General;

namespace Coco.Business.ValidationStrategies
{
    public class UserCoverValidationStrategy : IValidationStrategy
    {
        public UserCoverValidationStrategy()
        {
            Errors = new List<ErrorObject>();
        }

        public IEnumerable<ErrorObject> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            var data = value as UpdateUserPhotoModel;

            if (string.IsNullOrEmpty(data.PhotoUrl))
            {
                return false;
            }

            var image = ImageUtils.Base64ToImage(data.PhotoUrl);
            if(image.Width < 1000 || image.Height < 300)
            {
                return false;
            }

            return true;
        }
    }
}
