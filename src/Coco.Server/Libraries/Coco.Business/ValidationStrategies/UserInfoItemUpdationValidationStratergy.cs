﻿using System;
using System.Collections.Generic;
using System.Linq;
using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Core.Dtos.Identity;
using Coco.Core.Dtos.General;
using Coco.Core.Entities.Identity;

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

        public bool IsValid<T>(T value)
        {
            var model = value as UpdatePerItem;
            var propertyName = model.PropertyName;
            UserInfo userInfo;

            var ignoreCase = StringComparison.InvariantCultureIgnoreCase;

            if (propertyName.Equals(nameof(userInfo.Id), ignoreCase) || propertyName.Equals(nameof(userInfo.User), ignoreCase))
            {
                Errors = GetErrors(new NotSupportedException($"Not support {propertyName}"));
                return false;
            }
            
            if (propertyName.Equals(nameof(userInfo.PhoneNumber), ignoreCase))
            {
                _validationStrategyContext.SetStrategy(new PhoneValidationStrategy());
                bool isValid = (model.Value == null || string.IsNullOrEmpty(model.Value.ToString())) || _validationStrategyContext.Validate(model.Value);

                if (!isValid)
                {
                    Errors = _validationStrategyContext.Errors;
                }
            }
            else if (propertyName.Equals(nameof(userInfo.BirthDate), ignoreCase) && model.Value == null)
            {
                Errors = GetErrors(new NotSupportedException(nameof(userInfo.BirthDate)));
            }

            return Errors == null || !Errors.Any();
        }

        public IEnumerable<ErrorObject> GetErrors(Exception exception)
        {
            yield return new ErrorObject()
            {
                Message = exception.Message
            };
        }
    }
}