using System;
using System.Collections.Generic;
using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Model.General;

namespace Coco.Business.ValidationStrategies
{
    public class UserInfoItemUpdationValidationStratergy : IValidationStrategy
    {
        public IEnumerable<ErrorObject> Errors { get; set; }
        private readonly ValidationStrategyContext _validationStrategyContext;
        public UserInfoItemUpdationValidationStratergy(ValidationStrategyContext validationStrategyContext)
        {
            _validationStrategyContext = validationStrategyContext;
        }

        public bool IsValid<T>(T data)
        {
            var model = data as UpdatePerItem;
            UserInfo userInfo = new UserInfo();
            bool isValid = true;
            if (model.PropertyName.Equals(nameof(userInfo.PhoneNumber),
                StringComparison.InvariantCultureIgnoreCase))
            {
                _validationStrategyContext.SetStrategy(new PhoneValidationStrategy());
                isValid = (model.Value == null
                    || string.IsNullOrEmpty(model.Value.ToString()))
                    || _validationStrategyContext.Validate(model.Value);
            }
            else if (model.PropertyName.Equals(nameof(userInfo.BirthDate),
                StringComparison.InvariantCultureIgnoreCase))
            {
                isValid = model.Value != null;
            }
            // Check for photo & cover photo
            else if (model.PropertyName.Equals(nameof(userInfo.Photo), StringComparison.InvariantCultureIgnoreCase))
            {
                _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
                isValid = _validationStrategyContext.Validate(model.Value);
            }
            else if (model.PropertyName.Equals(nameof(userInfo.CoverPhoto), StringComparison.InvariantCultureIgnoreCase))
            {
                _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
                isValid = _validationStrategyContext.Validate(model.Value);
            }
            else if (model.PropertyName.Equals(nameof(userInfo.Id),
                StringComparison.InvariantCultureIgnoreCase)
                || model.PropertyName.Equals(nameof(userInfo.User),
                StringComparison.InvariantCultureIgnoreCase))
            {
                isValid = false;
            }

            return isValid;
        }

        private IEnumerable<ErrorObject> GetErrors()
        {
            return new List<ErrorObject>();
        }
    }
}
