using System.Collections.Generic;
using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Common.Utils;
using Coco.Entities.Model.General;

namespace Coco.Business.ValidationStrategies
{
    public class AvatarValidationStrategy : IValidationStrategy
    {
        public AvatarValidationStrategy()
        {
            Errors = new List<ErrorObject>();
        }

        public IEnumerable<ErrorObject> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            var data = value as UpdateAvatarModel;

            if (string.IsNullOrEmpty(data.PhotoUrl))
            {
                return false;
            }

            var image = ImageUtils.Base64ToImage(data.PhotoUrl);
            if(image.Width < 100 || image.Height < 100)
            {
                return false;
            }

            return true;
        }
    }
}
