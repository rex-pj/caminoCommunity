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
            var propertyName = model.PropertyName;
            UserInfo userInfo = new UserInfo();
            var ignoreCase = StringComparison.InvariantCultureIgnoreCase;
            bool isValid = true;
            if (propertyName.Equals(nameof(userInfo.PhoneNumber), ignoreCase))
            {
                _validationStrategyContext.SetStrategy(new PhoneValidationStrategy());
                isValid = (model.Value == null
                    || string.IsNullOrEmpty(model.Value.ToString()))
                    || _validationStrategyContext.Validate(model.Value);
            }
            else if (propertyName.Equals(nameof(userInfo.BirthDate), ignoreCase))
            {
                isValid = model.Value != null;
            }
            else if (propertyName.Equals(nameof(userInfo.Photo), ignoreCase) ||
                model.PropertyName.Equals(nameof(userInfo.CoverPhoto), ignoreCase))
            {
                isValid = false;
            }
            else if (propertyName.Equals(nameof(userInfo.Id), ignoreCase)
                || propertyName.Equals(nameof(userInfo.User), ignoreCase))
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
